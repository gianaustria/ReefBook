using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;

using System.Data;
using System.Data.SqlClient;

namespace Reefbook.App_Code
{
    public class Helper
    {
        public static string GetCon()
        {
            return ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;
        }

        public static string ToTitleCase(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        public static string FirstLetterToUpper(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }

        public static void SendEmail(string email, string subject, string message)
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.From = new MailAddress("reefbookmail@gmail.com", "no-reply");
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = message;
            emailMessage.IsBodyHtml = true;
            emailMessage.Priority = MailPriority.Normal;
            SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587);
            MailClient.EnableSsl = true;
            MailClient.Credentials = new System.Net.NetworkCredential("reefbookmail@gmail.com", "bookreef1");
            MailClient.Send(emailMessage);
        }

        public static void ReceiveEmail(string email, string subject, string message)
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.From = new MailAddress("reefbookmail@gmail.com", "Customer Feedback");
            emailMessage.To.Add(new MailAddress(email));
            emailMessage.Subject = subject;
            emailMessage.Body = message;
            emailMessage.IsBodyHtml = true;
            emailMessage.Priority = MailPriority.Normal;
            SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587);
            MailClient.EnableSsl = true;
            MailClient.Credentials = new System.Net.NetworkCredential("reefbookmail@gmail.com", "bookreef1");
            MailClient.Send(emailMessage);
        }

        /// <summary>
        /// Returns a hash value using a secured hash algorithm (SHA-2)
        /// </summary>
        public static string Hash(string phrase)
        {
            SHA512Managed HashTool = new SHA512Managed();
            Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(phrase));
            Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }

        /// <summary>
        /// Creates an audit log record based from user's activity and description
        /// </summary>
        /// <param name="type">e.g Login, Logout, Add, Update, Error</param>
        /// <param name="description">e.g. Logged in successfully, Added user.</param>
        public static void Log(string type, string description)
        {
            using (SqlConnection con = new SqlConnection(GetCon()))
            {
                con.Open();
                string query = @"INSERT INTO Logs VALUES
                    (@UserID, @LogType, @Description, @Timestamp);";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    if (HttpContext.Current.Session["profileID"] == null)
                        cmd.Parameters.AddWithValue("@UserID", 0);
                    else
                        cmd.Parameters.AddWithValue("@UserID",
                            HttpContext.Current.Session["profileID"].ToString());
                    cmd.Parameters.AddWithValue("@LogType", type);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static double GetPrice(int productID)
        {
            using (SqlConnection con = new SqlConnection(GetCon()))
            {
                con.Open();
                string query = @"SELECT Price FROM Products
                    WHERE ProductID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    //using (SqlDataReader data = cmd.ExecuteReader())
                    //{
                    //    double price = 0;
                    //    while (data.Read())
                    //    {
                    //        price = double.Parse(data["Price"].ToString());
                    //    }
                    //    return price;
                    //}

                    return Convert.ToDouble((decimal)cmd.ExecuteScalar()); // typecasting
                }
            }
        }

        public static bool IsExistingFromCart(int productID)
        {
            using (SqlConnection con = new SqlConnection(GetCon()))
            {
                con.Open();
                string query = @"SELECT RefNo FROM OrderDetails
                    WHERE OrderNo=@OrderNo AND profileID=@profileID
                    AND ProductID=@ProductID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@profileID", HttpContext.Current.Session["profileID"].ToString());
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    return cmd.ExecuteScalar() == null ? false : true;
                }
            }
        }

        public static void AddToCart(int productID, int quantity)
        {
            using (SqlConnection con = new SqlConnection(GetCon()))
            {
                con.Open();
                string query = "";
                if (IsExistingFromCart(productID))
                {
                    query = @"UPDATE OrderDetails SET Quantity = Quantity + @Quantity,
                        Amount = Amount + @Amount
                        WHERE OrderNo=@OrderNo AND profileID=@profileID
                        AND ProductID=@ProductID";
                }
                else
                {
                    query = @"INSERT INTO OrderDetails VALUES
                        (@OrderNo, @profileID, @ProductID,
                        @Quantity, @Amount, @Status)";
                }
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@OrderNo", 0);
                    cmd.Parameters.AddWithValue("@profileID", HttpContext.Current.Session["profileID"].ToString());
                    // replace 1 with HttpContext.Current.Session["userid"].ToString()
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@Amount", quantity * GetPrice(productID));
                    cmd.Parameters.AddWithValue("@Status", "In Cart");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void like(int id)
        {
            using (SqlConnection con = new SqlConnection(GetCon()))
            {
                con.Open();


                string query = @"INSERT INTO likes (postID, profileID) VALUES (@postID, @profileID)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@postID", id);
                    cmd.Parameters.AddWithValue("@profileID", HttpContext.Current.Session["profileID"].ToString());
                    // replace 1 with HttpContext.Current.Session["userid"].ToString()

                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlConnection con = new SqlConnection(GetCon()))
            {
                con.Open();


                string query = @"UPDATE profiles SET profileRank = CONVERT(varchar, profileRank) + 1 WHERE profileID = @profileID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@profileID", HttpContext.Current.Session["profileID"].ToString());
                    // replace 1 with HttpContext.Current.Session["userid"].ToString()

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void ValidateLogin()
        {
            if (HttpContext.Current.Session["profileID"] == null)
                HttpContext.Current.Response.Redirect("~/Profile/Login");
        }




    }
}