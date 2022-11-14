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
    public class ProfileController : Controller
    {
        // GET: Profile
        //public ActionResult Index()
        //{
        //    var list = new List<ProfileModel>();
        //    using (SqlConnection con = new SqlConnection(Helper.GetCon()))
        //    {
        //        con.Open();
        //        string query = @"SELECT profileID, profileLastname, profileFirstname, profileContactNo, profileRank, profileAddress,
        //                        profileRecentLocation FROM profiles";
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    list.Add(new ProfileModel
        //                    {
        //                        profileID = int.Parse(dr["profileID"].ToString()),
        //                        username = dr["username"].ToString(),
        //                        password = dr["password"].ToString(),
        //                        profileLastname = dr["profileLastname"].ToString(),
        //                        profileFirstname = dr["First Name"].ToString(),
        //                        profileContactNo = int.Parse(dr["Contact No"].ToString()),
        //                        profileRank = dr["Rank"].ToString(),
        //                        profileAddress = dr["Address"].ToString(),
        //                        profileRecentLocation = dr["Recent Location"].ToString(),

        //                    });

        //                }
        //            }
        //        }

        //    }
        //    return View();
        //}

        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Profile/Create
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }

        bool IsExisting(string email)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT Email FROM profiles
                    WHERE Email=@Email";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult SignUp(ProfileModel record)
        {
            if (IsExisting(record.username))
            {
                ViewBag.Error = "<div class='alert alert-danger col-lg-5'>Email already exists.</div>";
                return View(record);
            }
            else
            {
                using (SqlConnection con = new SqlConnection(Helper.GetCon()))
                {
                    con.Open();
                    string query = @"INSERT INTO profiles VALUES (@typeID, @username, @password,@image, @profileLastName, @profileFirstName, @profileContactNo, @profileRank, @profileStreet, @profileMunicipality, @profileCity, @profileRecentLocation)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@typeID", DBNull.Value);//customer
                        cmd.Parameters.AddWithValue("@username", record.username);
                        cmd.Parameters.AddWithValue("@password", record.password);
                        cmd.Parameters.AddWithValue("@image", "default-user-image.png");
                        cmd.Parameters.AddWithValue("@profileLastName", Helper.ToTitleCase(record.profileLastname));
                        cmd.Parameters.AddWithValue("@profileFirstName", Helper.ToTitleCase(record.profileFirstname));
                        cmd.Parameters.AddWithValue("@profileContactNo", "");
                        cmd.Parameters.AddWithValue("@profileRank", "1");
                        cmd.Parameters.AddWithValue("@profileStreet", "");
                        cmd.Parameters.AddWithValue("@profileMunicipality", "");
                        cmd.Parameters.AddWithValue("@profileCity", "");
                        cmd.Parameters.AddWithValue("@profileRecentLocation", "");
                        cmd.ExecuteNonQuery();
                        string message = "Welcome to ReefBook, " + record.profileFirstname + " " + record.profileLastname + "!<br/><br/>" +
                              "You have successfully created an account.<br/>" +
                              "Thank you for supporting Keanu Reeves.<br/><br/>" +
                              "Regards,<br/>" +
                              "RB Fam";

                        Helper.SendEmail(record.username, "Account Registration", message);
                    }
                    return RedirectToAction("Login", "Profile");
                }
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ProfileModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT profileID, typeID FROM profiles
                    WHERE Email=@Email AND Password=@Password";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Email", record.username);
                    cmd.Parameters.AddWithValue("@Password", record.password);
                    using (SqlDataReader data = cmd.ExecuteReader())
                    {
                        if (data.HasRows)
                        {
                            while (data.Read())
                            {
                                Session["profileID"] = data["profileID"].ToString();
                                Session["typeID"] = data["typeID"].ToString();
                            }
                            return RedirectToAction("Index", "Posts");
                        }
                        else
                        {
                            ViewBag.Error = "<div class='alert alert-danger col-lg-5'>Invalid email or password.</div>";
                            return View(record);
                        }
                    }
                }
            }
        }

        // POST: Profile/Create
        //[HttpPost]
        //public ActionResult AddProfileDetails(ProfileModel record)
        //{
        //    var list = new List<ProfileModel>();
        //    using (SqlConnection con = new SqlConnection(Helper.GetCon()))
        //    {
        //        con.Open();
        //        string query = @"INSERT INTO ReefBook VALUES(@profileID, @profileLastname, @profileFirstname, @profileContactNo, @profileRank, @profileAddress,
        //                       @profileRecentLocation, @userID, @accountID)";
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {

        //            cmd.Parameters.AddWithValue("@profileID", record.profileID);
        //            cmd.Parameters.AddWithValue("@profileLastname", record.profileLastname);
        //            cmd.Parameters.AddWithValue("@profileFirstname", record.profileFirstname);
        //            cmd.Parameters.AddWithValue("@profileContactNo", record.profileContactNo);
        //            cmd.Parameters.AddWithValue("@profileRank", record.profileRank);
        //            cmd.Parameters.AddWithValue("@profileAddress", record.profileAddress);
        //            cmd.Parameters.AddWithValue("@profileRecentLocation", record.profileRecentLocation);
        //            cmd.Parameters.AddWithValue("@userID", record.userID);
        //            cmd.Parameters.AddWithValue("@accountID", record.accountID);
        //            cmd.ExecuteNonQuery();
        //        }
        //        return RedirectToAction("Index");
        //    }
        //}

        // GET: Profile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Profile/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Profile/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
