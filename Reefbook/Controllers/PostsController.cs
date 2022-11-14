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
    public class PostsController : Controller
    {
        // GET: Posts






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


        public int getFriends()
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT COUNT(friendID) FROM friends where profileID = @profileID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"].ToString());
                    return (int)cmd.ExecuteScalar();
                }
            }
        }


        public int getPosts()
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT COUNT(postID) FROM posts where profileID = @profileID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"].ToString());
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public string getProfilePic()
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT images FROM profiles where profileID = @profileID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"].ToString());
                    return (string)cmd.ExecuteScalar();
                }
            }
        }



        public ActionResult Index()
        {

            if (Session["profileID"] == null)
            {
                return RedirectToAction("Login", "Profile");
            }
            else
            {


                ViewBag.User = Helper.ToTitleCase(GetUserData("profileFirstName")) + " " + Helper.ToTitleCase(GetUserData("profileLastName"));
                ViewBag.Posts = getPosts();
                ViewBag.Rank = GetUserData("profileRank");
                ViewBag.ProfilePic = getProfilePic();

                var list = new List<PostsModel>();
                using (SqlConnection con = new SqlConnection(Helper.GetCon()))
                {
                    con.Open();

                    string query = @"SELECT ps.postID, ps.profileID, ps.post,p.images, p.profileFirstName, p.profileLastName FROM posts ps 
                                 INNER JOIN profiles p ON ps.profileID = p.profileID
                                 ORDER BY postID DESC";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                list.Add(new PostsModel
                                {

                                    postID = int.Parse(dr["postID"].ToString()),
                                    image = dr["images"].ToString(),
                                    profileID = int.Parse(dr["profileID"].ToString()),
                                    FirstName = Helper.ToTitleCase(dr["profileFirstName"].ToString()),
                                    LastName = Helper.ToTitleCase(dr["profileLastName"].ToString()),
                                    post = dr["post"].ToString(),




                                });

                            }
                        }
                    }

                }

                return View(list);
            }
        }



        // GET: Posts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Posts/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        public ActionResult Add(PostsModel record)
        {
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO posts VALUES(@profileID, @post)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {


                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"].ToString());
                    cmd.Parameters.AddWithValue("@post", record.post);

                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index", "Posts");
            }
        }

        public ActionResult AddEvents()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        public ActionResult AddEvents(PostsModel record, HttpPostedFileBase image)
        {

            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO events VALUES(@title, @desc, @date, @image,@profileID)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@title", record.eventTitle);
                    cmd.Parameters.AddWithValue("@desc", record.eventDesc);
                    cmd.Parameters.AddWithValue("@date", record.eventDate);
                    cmd.Parameters.AddWithValue("@image",
                           DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName);
                    image.SaveAs(Server.MapPath("~/Images/Profiles/" +
                        DateTime.Now.ToString("yyyyMMddHHmmss-") + image.FileName));
                    cmd.Parameters.AddWithValue("@profileID", Session["profileID"].ToString());


                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index", "Events");
            }
        }


        public ActionResult Like(int? id)
        {

            Helper.like((int)id);
            return RedirectToAction("Index");
        }



        // GET: Posts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Posts/Edit/5
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

        // GET: Posts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Posts/Delete/5
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
