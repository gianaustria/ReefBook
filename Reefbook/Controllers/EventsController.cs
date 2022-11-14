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
    public class EventsController : Controller
    {
        // GET: Events
        public ActionResult Index()
        {
            var list = new List<EventsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"SELECT * FROM events";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new EventsModel
                            {

                                ID = int.Parse(dr["eventID"].ToString()),
                                eventTitle = dr["eventTitle"].ToString(),
                                eventDescription = dr["eventDescription"].ToString(),
                                image = dr["image"].ToString(),
                                eventDate = DateTime.Parse(dr["eventDate"].ToString())

                            });



                        }
                    }
                }

            }
            return View(list);
        }


        // GET: Events/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Events/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        public ActionResult Add(EventsModel record)
        {
            var list = new List<EventsModel>();
            using (SqlConnection con = new SqlConnection(Helper.GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO ReefBook VALUES(@eventID, @eventTitle, @eventDescription, @eventDate, @profileID, @imageID)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@eventID", record.ID);
                    cmd.Parameters.AddWithValue("@eventTitle", record.eventTitle);
                    cmd.Parameters.AddWithValue("@eventDescription", record.eventDescription);
                    cmd.Parameters.AddWithValue("@eventDate", record.eventDate);
                    cmd.Parameters.AddWithValue("@profileID", record.profileID);
                    cmd.Parameters.AddWithValue("@imageID", record.image);
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index");
            }

        }
        // GET: Events/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Events/Edit/5
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

        // GET: Events/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Events/Delete/5
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
