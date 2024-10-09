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
    public class PhansController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();


        // GET: Admin/Phans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phan phan = db.Phans.Find(id);
            if (phan == null)
            {
                return HttpNotFound();
            }
            return View(phan);
        }

        // GET: Admin/Phans/Create
        public ActionResult Create(int MaPhim)
        {
            ViewBag.Phim = db.Phims.Find(MaPhim);
            return View();
        }

        // POST: Admin/Phans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhim,TenPhan")] Phan phan)
        {
            if (ModelState.IsValid)
            {
                int MaUser = int.Parse(Session["ma"].ToString());
                phan.MaUserTao = MaUser;
                phan.MaUserSua = MaUser;
                phan.CreateAt = DateTime.Now;
                phan.UpdateAt = DateTime.Now;
                phan.isDel = false;

                db.Phans.Add(phan);
                db.SaveChanges();
                return RedirectToAction("Edit", "Phims", new {id = phan.MaPhim});
            }

            ViewBag.Phim = db.Phims.Find(phan.MaPhim);
            return View(phan);
        }

        // GET: Admin/Phans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phan phan = db.Phans.Find(id);
            if (phan == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MaPhim = new SelectList(db.Phims, "MaPhim", "Ten", phan.MaPhim);
            return View(phan);
        }

        // POST: Admin/Phans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int MaPhan,int MaPhim,string TenPhan)
        {
            Phan phan = db.Phans.Find(MaPhan);
            if (ModelState.IsValid)
            {
                phan.TenPhan = TenPhan;
                int MaUser = int.Parse(Session["ma"].ToString());
                phan.MaUserSua = MaUser;
                phan.UpdateAt = DateTime.Now;

                db.Entry(phan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit","Phims", new {id = MaPhim});
            }
            //ViewBag.MaPhim = new SelectList(db.Phims, "MaPhim", "Ten", phan.MaPhim);
            return View(phan);
        }

        // GET: Admin/Phans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phan phan = db.Phans.Find(id);
            if (phan == null)
            {
                return HttpNotFound();
            }
            return View(phan);
        }

        // POST: Admin/Phans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Phan phan = db.Phans.Find(id);
            db.Phans.Remove(phan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult setDel(int id)
        {
            Phan phan = db.Phans.Find(id);
            phan.isDel = true;
            db.Entry(phan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Phims", new { id = phan.MaPhim });
        }

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
