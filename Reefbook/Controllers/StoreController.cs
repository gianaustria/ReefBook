using Reefbook.App_Code;
using Reefbook.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Reefbook.Controllers
{
    public class StoreController : Controller
    {
        public List<CategoriesModel> GetCategories()
        {
            var list = new List<CategoriesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT c.CatID, c.Category, (SELECT Count(p.ProductID) FROM Products p WHERE p.CatID = c.CatID)
                          AS TotalCount FROM Categories c ORDER BY c.category";
                using (SqlCommand com = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = com.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CategoriesModel
                            {
                                CatID = int.Parse(data["CatID"].ToString()),
                                Category = data["Category"].ToString(),
                                TotalCount = int.Parse(data["TotalCount"].ToString())

                            });
                        }
                        return list;
                    }
                }
            }

        }

        public List<ProductsModel> GetProducts()
        {
            var list = new List<ProductsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT p.ProductID, p.Image, 
                    p.Name, p.Price, c.Category, p.CatID
                    FROM Products p 
                    INNER JOIN Categories c ON p.CatID = c.CatID
                    ORDER BY p.Name";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new ProductsModel
                            {
                                ID = int.Parse(data["ProductID"].ToString()),
                                Image = data["Image"].ToString(),
                                Name = data["Name"].ToString(),
                                Price = decimal.Parse(data["Price"].ToString()),
                                Category = data["Category"].ToString(),
                                CatID = int.Parse(data["CatID"].ToString())
                            });
                        }
                        return list;
                    }
                }
            }
        }
        // GET: Store
        public ActionResult Index()
        {
            Helper.ValidateLogin();
            if (Session["typeID"] == null || (string)Session["typeID"] == "" )
            {
                var list = new StoreViewModel();
                list.AllCategories = GetCategories();
                list.AllProducts = GetProducts();
                return View(list);
            }


            else
            {
                return RedirectToAction("AdminIndex");
            }


        }

        public ActionResult AdminIndex()
        {
            Helper.ValidateLogin();

            var list = new StoreViewModel();
            list.AllCategories = GetCategories();
            list.AllProducts = GetProducts();
            return View(list);





        }

        public ActionResult AddToCart(int? id)
        {
            if (id == null) //products is not selected
            {
                return RedirectToAction("Index");
            }

            Helper.AddToCart((int)id, 1);
            return RedirectToAction("Index");
        }

        public string GetUserData(string column)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT CONVERT(varchar, ISNULL(" + column + ",''))" +
                    "FROM profiles WHERE profileID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserID", Session["profileID"]);
                    return (string)cmd.ExecuteScalar();
                }
            }
        }

        public List<CartModel> GetCart()
        {
            var list = new List<CartModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT od.RefNo, od.ProductID,
                    p.Name, p.Image, p.Description, p.Price, od.Quantity,
                    od.Amount
                    FROM OrderDetails od
                    INNER JOIN Products p ON od.ProductID = p.ProductID
                    WHERE od.OrderNo=@OrderNo AND od.profileID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", Session["profileID"]);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {
                            list.Add(new CartModel
                            {
                                RefNo = int.Parse(data["RefNo"].ToString()),
                                ProductID = int.Parse(data["ProductID"].ToString()),
                                Name = data["Name"].ToString(),
                                Image = data["Image"].ToString(),
                                Description = data["Description"].ToString(),
                                Price = decimal.Parse(data["Price"].ToString()),
                                Quantity = int.Parse(data["Quantity"].ToString()),
                                Amount = decimal.Parse(data["Amount"].ToString())
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public double GetTotalAmount()
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT SUM(Amount) FROM OrderDetails
                    WHERE OrderNo=@OrderNo AND profileID=@UserID    
                    HAVING COUNT(RefNo) > 0";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@UserID", Session["profileID"]);
                    return cmd.ExecuteScalar() == null ? 0 : Convert.ToDouble((decimal)cmd.ExecuteScalar());
                }
            }
        }

        public ActionResult Cart()
        {
            Helper.ValidateLogin();
            var list = GetCart();
            ViewBag.Gross = (GetTotalAmount() * .88).ToString("#,##0.00");
            ViewBag.VAT = (GetTotalAmount() * .12).ToString("#,##0.00");
            ViewBag.Delivery = (GetTotalAmount() * .05).ToString("#,##0.00");
            ViewBag.Total = (GetTotalAmount() * 1.05).ToString("#,##0.00");
            return View(list);
        }

        public ActionResult Checkout()
        {
            var record = new OrdersViewModel();
            record.AllCartItems = GetCart();
            ViewBag.Gross = (GetTotalAmount() * .88).ToString("#,##0.00");
            ViewBag.VAT = (GetTotalAmount() * .12).ToString("#,##0.00");
            ViewBag.Delivery = (GetTotalAmount() * .05).ToString("#,##0.00");
            ViewBag.Total = (GetTotalAmount() * 1.05).ToString("#,##0.00");

            record.FN = GetUserData("profileFirstName");
            record.LN = GetUserData("profileLastName");
            record.Street = GetUserData("profileStreet");
            record.Municipality = GetUserData("profileMunicipality");
            record.City = GetUserData("profileCity");
            record.Phone = GetUserData("profileContactNo");

            return View(record);
        }
        [HttpPost]
        public ActionResult Checkout(OrdersViewModel record)
        {
            #region Step 1: Insert Order Record, Retrieve Last Record
            int orderNo = 0;
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Orders VALUES
                    (@DateOrdered, @PaymentMethod, @Status);
                    SELECT TOP 1 OrderNo FROM Orders
                    ORDER BY OrderNo DESC;";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@DateOrdered", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PaymentMethod", "Cash on Delivery");
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    orderNo = (int)cmd.ExecuteScalar();
                }
            }
            #endregion

            #region Step 2: Update Cart Items
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"UPDATE OrderDetails SET OrderNo=@OrderNo
                    WHERE OrderNo=@OrderNo AND profileID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                    cmd.Parameters.AddWithValue("@UserID", Session["profileID"]);
                    cmd.ExecuteNonQuery();
                }
            }
            #endregion

            #region Step 3: Update Customer Information
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"UPDATE profiles SET profileFirstName=@FirstName,
                    profileLastName=@LastName, profileStreet=@Street, 
                    profileMunicipality=@Municipality, profileCity=@City,
                    profileContactNo=@Phone
                    WHERE profileID=@UserID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                    cmd.Parameters.AddWithValue("@UserID", Session["profileID"]);
                    cmd.Parameters.AddWithValue("@FirstName", record.FN);
                    cmd.Parameters.AddWithValue("@LastName", record.LN);
                    cmd.Parameters.AddWithValue("@Street", record.Street);
                    cmd.Parameters.AddWithValue("@Municipality", record.Municipality);
                    cmd.Parameters.AddWithValue("@City", record.City);
                    cmd.Parameters.AddWithValue("@Phone", record.Phone);


                    cmd.ExecuteNonQuery();
                }
            }
            #endregion

            #region Step 4: Insert Delivery Record
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Deliveries VALUES
                    (@OrderNo, @Deadline, @DateDelivered, @Status);";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", orderNo);
                    cmd.Parameters.AddWithValue("@Deadline", DateTime.Now.AddDays(5));
                    cmd.Parameters.AddWithValue("@DateDelivered", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.ExecuteNonQuery();
                }
            }

            #endregion

            #region Step 5: Send Order Confirmation

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT od.OrderNo, od.Quantity, od.Amount, p.profileLastName, p.profileFirstName, o.DateOrdered,
                                  o.PaymentMethod FROM OrderDetails od
                                  INNER JOIN profiles p ON p.profileID = od.profileID
                                  INNER JOIN Orders o ON o.OrderNo = od.RefNo
                                  WHERE p.profileID = @profileID; UPDATE OrderDetails SET Status='Pending' WHERE profileID = @profileID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"]);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        while (data.Read())
                        {


                            record.OrderNo = data["OrderNo"].ToString();
                            record.Quantity = data["Quantity"].ToString();
                            record.LN = data["profileLastName"].ToString();
                            record.FN = data["profileFirstName"].ToString();
                            record.DateOrdered = data["DateOrdered"].ToString();
                            record.PaymentMethod = data["PaymentMethod"].ToString();
                            record.Amount = double.Parse(data["Amount"].ToString());


                        }



                    }
                }

                string query2 = @"DELETE FROM OrderDetails WHERE Status = 'Pending';";
                using (SqlCommand cmd = new SqlCommand(query2, con))
                {

                    cmd.ExecuteNonQuery();
                }
            }
            string message = "Order Confirmation<br/><br/><hr/><br/>" +
                          "Order #" + record.OrderNo + "<br/>" +
                          "Quantity: " + record.Quantity + "<br/>" +
                          "Date Ordered: " + record.DateOrdered + "<br/><br/>" +
                          "Name: " + record.FN + " " + record.LN + "<br/>" +
                          "Sub total: " + record.Amount + "<br/>" +
                          "Shipping Fee: " + record.Amount * .05 + "<br/>" +
                          "Total (VAT incl.): " + record.Amount * 1.05 + "<br/><br/>" +
                          "Payment Method: " + record.PaymentMethod + "<br/><hr/>" +
                          "Thank you for purchasing our products.";

            Helper.SendEmail(GetUserData("Email"), "Order Confirmation", message);

            #endregion

            return RedirectToAction("Index","Store");
        }
    }
}