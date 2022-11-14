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
    public class HomePageController : Controller
    {
        // GET: HomePage

        string GetUsername()
        {
            return "Sample";
        }

        public ActionResult HomeView()
        {
            ViewBag.User = GetUsername();
            return View();
        }

        public ActionResult ReefFeed()
        {
            return View();
        }

        public string GetUserData(string column)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT CONVERT(varchar, ISNULL(" + column + ",''))" +
                    "FROM profiles WHERE profileID=@profileID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"].ToString());
                    return (string)cmd.ExecuteScalar();
                }
            }
        }
        [HttpPost]
        public ActionResult SendMail()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
           
            
            return View();
        }

     
        public ActionResult Support()
        {
            return View();
        }

        public ActionResult UserProfile()
        {
            
            if (Session["profileID"] == null)
            {
                return RedirectToAction("Login", "Profile");
            }
            else
            {
                var list = new List<ProfileModel>();
                using (SqlConnection con = new SqlConnection(Helper.GetCon()))
                {
                    con.Open();
                    string query = @"SELECT profileID,images,profileLastName,profileFirstName,profileContactNo,profileStreet, profileMunicipality, profileCity,profileRecentLocation,profileRank FROM profiles WHERE profileID = @id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", int.Parse(Session["profileID"].ToString()));
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {

                            while (dr.Read())
                            {
                                list.Add(new ProfileModel
                                {

                                    profileID = int.Parse(Session["profileID"].ToString()),
                                    Image = dr["images"].ToString(),
                                    profileLastname = dr["profileLastName"].ToString(),
                                    profileFirstname = dr["profileFirstName"].ToString(),
                                    profileContactNo = dr["profileContactNo"].ToString(),
                                    Street = dr["profileStreet"].ToString(),
                                    Municipality = dr["profileMunicipality"].ToString(),
                                    City = dr["profileCity"].ToString(),
                                    profileRecentLocation = dr["profileRecentLocation"].ToString(),
                                    profileRank = dr["profileRank"].ToString()


                                });
                            }
                        }
                    }
                }
                return View(list);
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("UserProfile");
            }
            var record = new ProfileModel();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT images,profileLastName,profileFirstName,profileContactNo,profileStreet, profileMunicipality,profileCity,profileRecentLocation FROM profiles WHERE profileID = @profileid";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@profileID", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                record.Image = dr["images"].ToString();
                                record.profileLastname = dr["profileLastName"].ToString();
                                record.profileFirstname = dr["profileFirstName"].ToString();
                                record.profileContactNo = dr["profileContactNo"].ToString();
                                record.Street = dr["profileStreet"].ToString();
                                record.Municipality = dr["profileMunicipality"].ToString();
                                record.City = dr["profileCity"].ToString();
                                record.profileRecentLocation = dr["profileRecentLocation"].ToString();



                            }
                            return View(record);
                        }

                        else
                        {
                            return RedirectToAction("UserProfile");
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id, ProfileModel record, HttpPostedFileBase image)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(Helper.GetCon()))
                {
                    con.Open();
                    string query = @"UPDATE profiles SET images=@image, profileLastName=@LN,
                profileFirstName=@FN, profileContactNo=@contactno,
                profileStreet=@st, profileMunicipality=@municipality, profileCity=@CT, profileRecentLocation=@recloc WHERE profileID=@profileID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Image",
                            DateTime.Now.ToString("yyyyMMddHHmm-") + image.FileName);
                        image.SaveAs(Server.MapPath("~/Images/Profiles/" +
                            DateTime.Now.ToString("yyyyMMddHHmm-") + image.FileName));
                        cmd.Parameters.AddWithValue("@LN", record.profileLastname);
                        cmd.Parameters.AddWithValue("@FN", record.profileFirstname);
                        cmd.Parameters.AddWithValue("@contactno", record.profileContactNo);
                        cmd.Parameters.AddWithValue("@st", record.Street);
                        cmd.Parameters.AddWithValue("@municipality", record.Municipality);
                        cmd.Parameters.AddWithValue("@CT", record.City);
                        cmd.Parameters.AddWithValue("@recloc", record.profileRecentLocation);

                        cmd.Parameters.AddWithValue("@profileID", Session["profileID"]);
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("UserProfile");
                    }
                }
            }
            catch
            {
                ViewBag.Error = "<div class='alert alert-danger'>Required fields must be filled out.</div>";
                return View();
            }

        }


        public ActionResult ProcessRequest()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }
    }
}