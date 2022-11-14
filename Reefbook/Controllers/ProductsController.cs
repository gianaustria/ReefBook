using Reefbook.App_Code;
using Reefbook.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Reefbook.Models.ProductsModel;

namespace Reefbook.Controllers
{
    public class ProductsController : Controller
    {

        public List<CategoriesModel> GetCategories()
        {
            var list = new List<CategoriesModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"Select CatID, Category FROM Categories ORDER BY Category";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new CategoriesModel
                            {
                                CatID = int.Parse(dr["CatID"].ToString()),
                                Category = dr["Category"].ToString()
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public ActionResult Add()
        {
            //Helper.ValidateLogin();
            var record = new ProductsModel();
            record.Categories = GetCategories();
            return View(record);
        }

        [HttpPost]
        public ActionResult Add(ProductsModel record, HttpPostedFileBase image)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO products VALUES
                    (@Name, @CatID, @Code, @Description,
                    @Image, @Price, @IsFeatured, @Available,
                    @Critical, @Maximum, @Status, @DateAdded, @DateModified)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", record.Name);
                    cmd.Parameters.AddWithValue("@CatID", record.CatID);
                    cmd.Parameters.AddWithValue("@Code", record.Code);
                    cmd.Parameters.AddWithValue("@Description", record.Description);
                    cmd.Parameters.AddWithValue("@Image",
                        DateTime.Now.ToString("yyyyMMddHHmm-") + image.FileName);
                    image.SaveAs(Server.MapPath("~/Images/Products/" +
                        DateTime.Now.ToString("yyyyMMddHHmm-") + image.FileName));
                    cmd.Parameters.AddWithValue("@Price", record.Price);
                    cmd.Parameters.AddWithValue("@IsFeatured", record.IsFeatured ? "Yes" : "No");
                    cmd.Parameters.AddWithValue("@Available", 100);
                    cmd.Parameters.AddWithValue("@Critical", record.Critical);
                    cmd.Parameters.AddWithValue("@Maximum", record.Maximum);
                    cmd.Parameters.AddWithValue("@Status", "Active");
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();
                    Helper.Log("Add", "Added a product record");

                    return RedirectToAction("Index", "Store");
                }
            }
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}


#region
//   // GET: Products
//   public ActionResult Index()
//   {
//       var list = new List<ProductsModel>();
//       using (SqlConnection con = new SqlConnection(Helper.GetCon()))
//       {
//           con.Open();
//           string query = @"SELECT productID, productName, productDescription, productPrice, productQty, profileID FROM products";
//           using (SqlCommand cmd = new SqlCommand(query, con))
//           {
//               using (SqlDataReader dr = cmd.ExecuteReader())
//               {
//                   while (dr.Read())
//                   {
//                       list.Add(new ProductsModel
//                       {

//                           productID = int.Parse(dr["Image ID"].ToString()),
//                           productDescription = dr["Post ID"].ToString(),
//                           productName = dr["Profile ID"].ToString(),
//                           productPrice = int.Parse(dr["Profile ID"].ToString()),
//                           productQty = int.Parse(dr["Profile ID"].ToString()),
//                           profileID = int.Parse(dr["Profile ID"].ToString()),


//                       });

//                   }
//               }
//           }

//       }
//       return View();
//   }

//   // POST: Products/Create
//   [HttpPost]
//   public ActionResult Add(ProductsModel record)
//   {
//       var list = new List<ProductsModel>();
//       using (SqlConnection con = new SqlConnection(Helper.GetCon()))
//       {
//           con.Open();
//           string query = @"INSERT INTO ReefBook VALUES(@productID, @productName, @productDescription, @productPrice, @productQty, @profileID)";
//           using (SqlCommand cmd = new SqlCommand(query, con))
//           {

//               cmd.Parameters.AddWithValue("@productID", record.productID);
//               cmd.Parameters.AddWithValue("@productName", record.productName);
//               cmd.Parameters.AddWithValue("@productDescription", record.productDescription);
//               cmd.Parameters.AddWithValue("@productPrice", record.productPrice);
//               cmd.Parameters.AddWithValue("@productQty", record.productQty);
//               cmd.Parameters.AddWithValue("@profileID", record.profileID); ;
//               cmd.ExecuteNonQuery();
//           }
//           return RedirectToAction("Index");
//       }   
//   }


//eturn View(record);
//   }
#endregion


