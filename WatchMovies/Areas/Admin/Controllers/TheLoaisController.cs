using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;
using PagedList;

namespace WatchMovies.Areas.Admin.Controllers
{
    public class TheLoaisController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/TheLoais
        public ActionResult Index(int? page)
        {
            ViewBag.TheLoai = "active";
            var list = db.TheLoais.OrderByDescending(c=>c.MaTheLoai).ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/TheLoais/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.TheLoai = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            return View(theLoai);
        }

        // GET: Admin/TheLoais/Create
        public ActionResult Create()
        {
            ViewBag.TheLoai = "active";
            return View();
        }

        // POST: Admin/TheLoais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaTheLoai,Ten")] TheLoai theLoai)
        {
            ViewBag.TheLoai = "active";

            if (ModelState.IsValid)
            {
                db.TheLoais.Add(theLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(theLoai);
        }

        // GET: Admin/TheLoais/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.TheLoai = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            return View(theLoai);
        }

        // POST: Admin/TheLoais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTheLoai,Ten")] TheLoai theLoai)
        {
            ViewBag.TheLoai = "active";

            if (ModelState.IsValid)
            {
                db.Entry(theLoai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(theLoai);
        }

        // GET: Admin/TheLoais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            try
            {
                db.TheLoais.Remove(theLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                //thông báo lỗi nếu không xóa được
                string message = "Không thể xóa do có phẩn tử con!";
                return Content("<script>alert('" + message + "'); window.location.href = '/Admin/TheLoais';</script>");
            } 
        }
    }
}
