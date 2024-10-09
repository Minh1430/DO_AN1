using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;

namespace WatchMovies.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(int MaUser)
        {
            WatchMoviesContext db = new WatchMoviesContext();
            User user = db.Users.Find(MaUser);
            db.Dispose();
            return PartialView(user);
        }
        [HttpPost]
        public bool Update(int MaUser, string HoTen, string Email, string SoDienThoai, HttpPostedFileBase hinhAnhFile)
        {
            WatchMoviesContext db = new WatchMoviesContext();
            User user = db.Users.Find(MaUser);
            if (hinhAnhFile != null && hinhAnhFile.ContentLength > 0)
            {
                var imagePath = Path.Combine(Server.MapPath("~/Content/img/User"), Path.GetFileName(hinhAnhFile.FileName));
                hinhAnhFile.SaveAs(imagePath);
                user.HinhAnh = hinhAnhFile.FileName;
            }
            user.Ten = HoTen;
            user.Email = Email;
            user.SDT = SoDienThoai;
            try
            {
                Session["Ten"] = HoTen;
                Session["HinhAnh"] = hinhAnhFile.FileName;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                db.Dispose();
                return true;
            }
            catch
            {
                db.Dispose();
                return false;
            }
            
        }
        [HttpPost]
        public ActionResult changePass(int MaUser, string PassWord, string NewPassWord, string ReNewPassWord)
        {
            WatchMoviesContext db = new WatchMoviesContext();
            User user = db.Users.Find(MaUser);
            PassWord = PassWord + "gia";
            if (user != null && BCrypt.Net.BCrypt.Verify(PassWord, user.Password) && !user.isDel)
            {
                if(NewPassWord == ReNewPassWord)
                {
                    NewPassWord = NewPassWord + "gia";
                    user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassWord, BCrypt.Net.BCrypt.GenerateSalt());
                    try
                    {
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        db.Dispose();
                        return Content("OK");
                    }
                    catch
                    {
                        db.Dispose();
                        return Content("Mật khẩu mới chưa được lưu!");
                    }
                }
                else
                {
                    db.Dispose();
                    return Content("Mật khẩu mới không trùng khớp!");
                }
            }
            else
            {
                db.Dispose();
                return Content("Mật khẩu cũ không đúng!");
            }
        }
    }
}