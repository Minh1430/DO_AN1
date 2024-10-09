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
    public class ChatLuongsController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/ChatLuongs
        public ActionResult Index(int? page)
        {
            ViewBag.ChatLuong = "active";
            var list = db.ChatLuongs.OrderByDescending(c=>c.MaChatLuong).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/ChatLuongs/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.ChatLuong = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatLuong chatLuong = db.ChatLuongs.Find(id);
            if (chatLuong == null)
            {
                return HttpNotFound();
            }
            return View(chatLuong);
        }

        // GET: Admin/ChatLuongs/Create
        public ActionResult Create()
        {
            ViewBag.ChatLuong = "active";
            return View();
        }

        // POST: Admin/ChatLuongs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaChatLuong,Ten")] ChatLuong chatLuong)
        {
            ViewBag.ChatLuong = "active";

            if (ModelState.IsValid)
            {
                db.ChatLuongs.Add(chatLuong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chatLuong);
        }

        // GET: Admin/ChatLuongs/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.ChatLuong = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatLuong chatLuong = db.ChatLuongs.Find(id);
            if (chatLuong == null)
            {
                return HttpNotFound();
            }
            return View(chatLuong);
        }

        // POST: Admin/ChatLuongs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaChatLuong,Ten")] ChatLuong chatLuong)
        {
            ViewBag.ChatLuong = "active";

            if (ModelState.IsValid)
            {
                db.Entry(chatLuong).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chatLuong);
        }

        // GET: Admin/ChatLuongs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChatLuong chatLuong = db.ChatLuongs.Find(id);
            if (chatLuong == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.ChatLuongs.Remove(chatLuong);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                // thông báo lỗi nếu không xóa được
                string message = "Không thể xóa do có phẩn tử con!";
                return Content("<script>alert('" + message + "'); window.location.href = '/Admin/ChatLuongs';</script>");
            }
        }

        // POST: Admin/ChatLuongs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ChatLuong chatLuong = db.ChatLuongs.Find(id);
        //    db.ChatLuongs.Remove(chatLuong);
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
