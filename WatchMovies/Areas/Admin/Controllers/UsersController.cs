using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;

namespace WatchMovies.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/Users
        public ActionResult Index(int? page)
        {
            ViewBag.User = "active";
            var list = db.Users.Where(u=>u.isDel==false).OrderByDescending(u=>u.MaUser).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.User = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.User = "active";
            ViewBag.Quyens = db.Quyens.ToList();
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaUser,Ten,Email,Password")] User user, List<int> selectedQuyens)
        {
            if (ModelState.IsValid)
            {
                ViewBag.User = "active";

                db.Users.Add(user);
                db.SaveChanges();
                if (selectedQuyens != null)
                {
                    foreach (var quyenId in selectedQuyens)
                    {
                        var quyenUser = new Quyen_User { MaUser = user.MaUser, MaQuyen = quyenId };
                        db.Quyen_Users.Add(quyenUser);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.User = "active";
            ViewBag.Quyens = db.Quyens.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaUser,Ten,Email,Password")] User user, List<int> selectedQuyens)
        {
            ViewBag.User = "active";
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                if (selectedQuyens != null)
                {
                    // Xóa tất cả quyền cũ của user
                    var existingQuyens = db.Quyen_Users.Where(tp => tp.MaUser == user.MaUser).ToList();
                    db.Quyen_Users.RemoveRange(existingQuyens);
                    db.SaveChanges();
                    // Thêm quyền mới
                    foreach (var quyenId in selectedQuyens)
                    {
                        var quyenUser = new Quyen_User { MaUser = user.MaUser, MaQuyen = quyenId };
                        db.Quyen_Users.Add(quyenUser);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult setDel(int id)
        {
            User user = db.Users.Find(id);
            user.isDel = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            try
            {
                //db.Users.Remove(user);
                user.isDel=true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                string message = "Không thể xóa do có phẩn tử con!";
                return Content("<script>alert('" + message + "'); window.location.href = '/Admin/Users';</script>");
            }
        }

        // POST: Admin/Users/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    User user = db.Users.Find(id);
        //    db.Users.Remove(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
