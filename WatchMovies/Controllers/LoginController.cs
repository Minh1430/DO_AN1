using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;

namespace WatchMovies.Controllers
{
    public class LoginController : Controller
    {
        protected WatchMoviesContext db = new WatchMoviesContext();
        public string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            StringBuilder stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
        public static string GetApp(string key)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }

            return "";
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        //đăng nhập
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            //cộng thêm chuỗi vào password sau đó kiểm tra mật khảu
            var x = db.Users.Where(u=>u.Email == Email).FirstOrDefault();
            Password = Password + "gia";
            if (x == null || !BCrypt.Net.BCrypt.Verify(Password, x.Password) || x.isDel)
            {
                ViewBag.LoginError = "Sai Email hoặc Password!";
                return View();
            }
            else
            {
                //kiểm tra quyền của người dùng
                Session["Ma"] = x.MaUser;
                Session["Ten"] = x.Ten;
                Session["HinhAnh"] = x.HinhAnh;
                var quyenList = x.Quyen_Users.ToList();
                if (quyenList.Count() == 1)
                {
                    if (quyenList[0].Quyen.MaQuyen == 2)
                        Session["role"] = "User";
                    else
                        Session["role"] = "Admin";
                }
                else
                    Session["role"] = "Admin";

                //nếu trước đó đang xem phim sẽ chuyển về phim vừa xem
                if (Session["MaPhim"]!=null)
                {
                    int MaPhim = int.Parse(Session["MaPhim"].ToString());
                    Session["MaPhim"] = null;
                    return RedirectToAction("Details","Films", new { id =  MaPhim});
                }
                return RedirectToAction("Index", "Home");
            }
        }


        //đăng xuất
        public ActionResult Logout()
        {
            Session["Ma"] = null;
            Session["Ten"] = null;
            Session["MaPhim"] = null;
            Session["role"] = null;
            Session["HinhAnh"] = null;
            return RedirectToAction("Index", "Home");
        }
        public ActionResult signup()
        {
            return View();
        }
        //đăng ký
        [HttpPost]
        public ActionResult signup(string Name, string Email, string SDT, string Password)
        {
            //khởi tạo user mới
            User user = new User();
            Password = Password + "gia";
            var x = db.Users.Where(u => u.Email==Email || u.SDT == SDT).FirstOrDefault();
            if(x == null)
            {
                user.Ten = Name;
                user.HinhAnh = "user.png";
                user.Email = Email;
                user.SDT = SDT;
                user.isDel = false;
                //mã hóa password
                user.Password = BCrypt.Net.BCrypt.HashPassword(Password, BCrypt.Net.BCrypt.GenerateSalt());
            }
            else
            {
                ViewBag.SignUpError = "Email hoặc SDT đã được sử dụng!";
                return View(user);
            }
            try
            {
                //thêm user mới và set quyền User cho người dùng
                db.Users.Add(user);
                db.SaveChanges();
                var quyenUer = new Quyen_User { MaUser = user.MaUser, MaQuyen = 2 };
                db.Quyen_Users.Add(quyenUer);
                db.SaveChanges();
                return View("Login");
            }catch
            {
                return View();
            }
            
        }

        public ActionResult forgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult forgotPassword(string email, string SDT)
        {
            string newPassWord = GenerateRandomString(6);
            User user = db.Users.Where(u => u.Email == email && u.SDT == SDT).FirstOrDefault();
            if(user == null)
            {
                ViewBag.ForgotPasswordError = "Thông tin không chính xác!";
                return View();
            }
            else
            {
                var fromEmailAddress = GetApp("FromEmailAddress");
                var fromEmailDisplayName = GetApp("FromEmailDisplayName");
                var fromEmailPassword = GetApp("FromEmailPassword");
                var smtpHost = GetApp("SMTPHost");
                var smtpPort = GetApp("SMTPPort");


                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.HeadersEncoding = System.Text.Encoding.UTF8;
                msg.SubjectEncoding = System.Text.Encoding.UTF8;
                msg.From = new MailAddress(fromEmailAddress, fromEmailDisplayName);
                msg.To.Add(email);

                msg.Subject = "Cấp lại mật khẩu cho tài khoản FilxGo";

                msg.IsBodyHtml = true;
                msg.Body = "Mật khẩu mới của bạn là: " + newPassWord;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                        | SecurityProtocolType.Tls11
                                        | SecurityProtocolType.Tls12;

                var client = new SmtpClient();

                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
                client.Host = smtpHost;
                client.EnableSsl = true;
                client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
                //client.Timeout = 20000;
                client.TargetName = "STARTTLS/smtp.office365.com";
                try
                {
                    client.Send(msg);
                    newPassWord =  newPassWord + "gia";
                    user.Password = BCrypt.Net.BCrypt.HashPassword(newPassWord, BCrypt.Net.BCrypt.GenerateSalt());
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    msg.Dispose();
                    return View("Login");
                }
                catch
                {
                    return View();
                }
                
            }
        }
    }
}