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
    public class ReportsController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/Reports
        public ActionResult Index(int? page)
        {
            ViewBag.Report = "active";
            var reports = db.Reports.Include(r => r.Comment).Include(r => r.User).OrderBy(r=>r.MaReport);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reports.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Admin/Reports/Create
        public ActionResult Create()
        {
            ViewBag.MaComment = new SelectList(db.Comments, "MaComment", "NoiDungComment");
            ViewBag.MaUser = new SelectList(db.Users, "MaUser", "Ten");
            return View();
        }

        // POST: Admin/Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MaUser,MaComment,NgayTao")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaComment = new SelectList(db.Comments, "MaComment", "NoiDungComment", report.MaComment);
            ViewBag.MaUser = new SelectList(db.Users, "MaUser", "Ten", report.MaUser);
            return View(report);
        }

        // GET: Admin/Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaComment = new SelectList(db.Comments, "MaComment", "NoiDungComment", report.MaComment);
            ViewBag.MaUser = new SelectList(db.Users, "MaUser", "Ten", report.MaUser);
            return View(report);
        }

        // POST: Admin/Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MaUser,MaComment,NgayTao")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaComment = new SelectList(db.Comments, "MaComment", "NoiDungComment", report.MaComment);
            ViewBag.MaUser = new SelectList(db.Users, "MaUser", "Ten", report.MaUser);
            return View(report);
        }

        // GET: Admin/Reports/Delete/5
        public ActionResult Delete(int MaPhim, int MaComment, bool Xoa)
        {
            var reports = db.Reports.Where(r=>r.MaComment==MaComment).ToList();
            db.Reports.RemoveRange(reports);
            db.SaveChanges();
            if (Xoa)
            {
                var comment = db.Comments.Find(MaComment);
                db.Comments.Remove(comment);
                var phim = db.Phims.Find(MaPhim);
                phim.SoLuongComment--;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: Admin/Reports/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Report report = db.Reports.Find(id);
        //    db.Reports.Remove(report);
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
