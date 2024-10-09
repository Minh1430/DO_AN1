using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;
using PagedList;

namespace WatchMovies.Areas.Admin.Controllers
{
    public class BaiDangsController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/BaiDangs
        public ActionResult Index(int? page)
        {
            ViewBag.BaiDang = "active";
            var baiDangs = db.BaiDangs.Include(b => b.UserTao).OrderByDescending(b=>b.MaBaiDang);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(baiDangs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/BaiDangs/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.BaiDang = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiDang baiDang = db.BaiDangs.Find(id);
            if (baiDang == null)
            {
                return HttpNotFound();
            }
            return View(baiDang);
        }

        // GET: Admin/BaiDangs/Create
        public ActionResult Create()
        {
            ViewBag.BaiDang = "active";
            return View();
        }

        // POST: Admin/BaiDangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaBaiDang,Ten,NoiDung,HinhAnh,DaoDien,DienVien,MaUserTao,NgayTao")] BaiDang baiDang, HttpPostedFileBase hinhAnhFile)
        {
            ViewBag.BaiDang = "active";
            try
            {
                baiDang.MaUserTao = int.Parse(Session["ma"].ToString());
                baiDang.NgayTao = DateTime.Now;
                if (hinhAnhFile != null && hinhAnhFile.ContentLength > 0)
                {
                    var imagePath = Path.Combine(Server.MapPath("~/Content/img/post"), Path.GetFileName(hinhAnhFile.FileName));
                    hinhAnhFile.SaveAs(imagePath);
                    baiDang.HinhAnh = hinhAnhFile.FileName;
                }
                else { baiDang.HinhAnh = "no-image"; }
                db.BaiDangs.Add(baiDang);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(baiDang);
            }  
        }

        // GET: Admin/BaiDangs/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.BaiDang = "active";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiDang baiDang = db.BaiDangs.Find(id);
            if (baiDang == null)
            {
                return HttpNotFound();
            }
            return View(baiDang);
        }

        // POST: Admin/BaiDangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaBaiDang,Ten,NoiDung,HinhAnh,DaoDien,DienVien,MaUserTao,NgayTao")] BaiDang baiDang, HttpPostedFileBase hinhAnhFile)
        {
            ViewBag.BaiDang = "active";

            BaiDang x = db.BaiDangs.Find(baiDang.MaBaiDang);
            x.Ten = baiDang.Ten;
            x.NoiDung = baiDang.NoiDung;
            x.DaoDien = baiDang.DaoDien;
            x.DienVien = baiDang.DienVien;
            try
            {
                // Xử lý file ảnh
                if (hinhAnhFile != null && hinhAnhFile.ContentLength > 0)
                {
                    var imagePath = Path.Combine(Server.MapPath("~/Content/img/post"), Path.GetFileName(hinhAnhFile.FileName));
                    hinhAnhFile.SaveAs(imagePath);
                    x.HinhAnh = hinhAnhFile.FileName;
                }
                db.Entry(x).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(baiDang);
            }
            
        }

        // GET: Admin/BaiDangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiDang baiDang = db.BaiDangs.Find(id);
            if (baiDang == null)
            {
                return HttpNotFound();
            }
            db.BaiDangs.Remove(baiDang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/BaiDangs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    BaiDang baiDang = db.BaiDangs.Find(id);
        //    db.BaiDangs.Remove(baiDang);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

    }
}
