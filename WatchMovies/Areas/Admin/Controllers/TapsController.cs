using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;

namespace WatchMovies.Areas.Admin.Controllers
{
    public class TapsController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/Taps
        public ActionResult Index(int MaPhan)
        {
            ViewBag.Phan = db.Phans.Find(MaPhan);
            return View(db.Taps.Where(t=>t.MaPhan==MaPhan && t.isDel == false).ToList());
        }

        // GET: Admin/Taps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tap tap = db.Taps.Find(id);
            if (tap == null)
            {
                return HttpNotFound();
            }
            return View(tap);
        }

        // GET: Admin/Taps/Create
        public ActionResult Create(int MaPhan)
        {
            ViewBag.Phan = db.Phans.Find(MaPhan);
            return View();
        }

        // POST: Admin/Taps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhan,Ten,Link")] Tap tap)
        {
            if (ModelState.IsValid)
            {
                int MaUser = int.Parse(Session["ma"].ToString());
                tap.MaUserTao = MaUser;
                tap.MaUserSua = MaUser;
                tap.CreateAt = DateTime.Now;
                tap.UpdatedAt = DateTime.Now;
                tap.isDel = false;

                db.Taps.Add(tap);
                db.Entry(tap).Reference(c => c.Phan).Load();
                db.SaveChanges();

                List<int> listUser = db.YeuThichs.Where(y=>y.MaPhim == tap.Phan.MaPhim).Select(u=>u.MaUser).ToList();
                foreach (var item in listUser)
                {
                    ThongBao tb = new ThongBao();
                    tb.MaUser = item;
                    tb.NoiDung = "Phim " + tap.Phan.Phim.Ten +" " + tap.Phan.TenPhan + "đã có tập mới: " + tap.Ten;
                    tb.Link= "/Films/Details?id=" + tap.Phan.MaPhim + "&MaTap=" + tap.MaTap;
                    tb.CreateAt = DateTime.Now;
                    tb.DaXem = false;

                    db.ThongBaos.Add(tb);
                }
                db.SaveChanges();

                return RedirectToAction("Edit", "Phims", new { id = tap.Phan.MaPhim });
            }

            return View(tap);
        }

        // GET: Admin/Taps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tap tap = db.Taps.Find(id);
            if (tap == null)
            {
                return HttpNotFound();
            }
            return View(tap);
        }

        // POST: Admin/Taps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int MaTap,string Ten,string Link)
        {
            Tap tap = db.Taps.Find(MaTap);
            if (ModelState.IsValid)
            {
                tap.Ten = Ten;
                tap.Link = Link;
                tap.MaUserSua = int.Parse(Session["Ma"].ToString());
                tap.UpdatedAt = DateTime.Now;
                db.Entry(tap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new {MaPhan = tap.MaPhan});
            }
            return View(tap);
        }

        // GET: Admin/Taps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tap tap = db.Taps.Find(id);
            if (tap == null)
            {
                return HttpNotFound();
            }
            return View(tap);
        }

        // POST: Admin/Taps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tap tap = db.Taps.Find(id);
            db.Taps.Remove(tap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult setDel(int id)
        {
            Tap tap = db.Taps.Find(id);
            tap.isDel = true;
            db.Entry(tap).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { MaPhan = tap.MaPhan });
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
