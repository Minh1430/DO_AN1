using PagedList;
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

namespace WatchMovies.Areas.Admin.Controllers
{
    public class PhimsController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();

        // GET: Admin/Phims
        public ActionResult Index(int? page)
        {
            ViewBag.Phim = "active";
            var phims = db.Phims.Where(p=>p.isDel==false).Include(p => p.ChatLuong).OrderByDescending(p=>p.MaPhim);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(phims.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Phims/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.Phim = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            return View(phim);
        }

        // GET: Admin/Phims/Create
        public ActionResult Create()
        {
            ViewBag.Phim = "active";
            ViewBag.MaChatLuong = new SelectList(db.ChatLuongs, "MaChatLuong", "Ten");
            ViewBag.TheLoais = db.TheLoais.ToList();
            return View();
        }

        // POST: Admin/Phims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhim,Ten,NoiDung,Link,HinhAnh,DaoDien,DienVien,QuocGia,NamSX,ThoiLuong,MaChatLuong, LoaiPhim")] Phim phim, List<int> selectedTheLoais, HttpPostedFileBase hinhAnhFile)
        {
            ViewBag.Phim = "active";
            try
            {
                phim.HinhAnh = "";
                phim.LuotXem = 0;
                phim.SoSao = 10;
                phim.SoLuongComment = 0;
                phim.SoLuongVote = 0;
                phim.MaUserTao = int.Parse(Session["ma"].ToString());
                phim.MaUserSua = 0;
                phim.CreateAt = DateTime.Now;
                phim.UpdateAt = DateTime.Now;
                phim.isDel = false;
                // Xử lý file ảnh
                if (hinhAnhFile != null && hinhAnhFile.ContentLength > 0)
                {
                    var imagePath = Path.Combine(Server.MapPath("~/Content/img/film"), Path.GetFileName(hinhAnhFile.FileName));
                    hinhAnhFile.SaveAs(imagePath);
                    phim.HinhAnh = hinhAnhFile.FileName;
                }
                if (phim.LoaiPhim == 2)
                    phim.Link = null;
                db.Phims.Add(phim);
                db.SaveChanges();
                //Thêm thể loại phim
                if (selectedTheLoais != null)
                {
                    foreach (var theLoaiId in selectedTheLoais)
                    {
                        var theLoaiPhim = new TheLoai_Phim { MaPhim = phim.MaPhim, MaTheLoai = theLoaiId };
                        db.TheLoai_Phims.Add(theLoaiPhim);
                    }
                    db.SaveChanges();
                }
                //Thêm phần
                if (phim.LoaiPhim == 2)
                {
                    int num = int.Parse(Request["num"].ToString());
                    for(int i = 1; i < num; i++)
                    {
                        var item = new Phan();
                        item.MaPhim = phim.MaPhim;
                        item.TenPhan = "Phần " + i;
                        item.CreateAt = DateTime.Now;
                        item.UpdateAt = DateTime.Now;
                        item.MaUserTao = phim.MaUserTao;
                        item.MaUserSua = phim.MaUserTao;
                        item.isDel = false;
                        db.Phans.Add(item);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.MaChatLuong = new SelectList(db.ChatLuongs, "MaChatLuong", "Ten", phim.MaChatLuong);
                ViewBag.TheLoais = db.TheLoais.ToList();
                return View(phim);
            }

        }

        // GET: Admin/Phims/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Phim = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaChatLuong = new SelectList(db.ChatLuongs, "MaChatLuong", "Ten", phim.MaChatLuong);
            ViewBag.Phans = db.Phans.Where(t=>t.MaPhim==id && t.isDel==false).ToList();
            ViewBag.TheLoais = db.TheLoais.ToList();
            return View(phim);
        }

        // POST: Admin/Phims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhim,Ten,Link,NoiDung,HinhAnh,DaoDien,DienVien,QuocGia,NamSX,ThoiLuong,LuotXem,SoLuongVote,SoSao,SoLuongComment,MaChatLuong,LoaiPhim")] Phim phim, List<int> selectedTheLoais, HttpPostedFileBase hinhAnhFile)
        {
            ViewBag.Phim = "active";

            Phim p = db.Phims.Find(phim.MaPhim);
            p.Ten = phim.Ten;
            p.Link = phim.Link;
            p.NoiDung=phim.NoiDung;
            p.DaoDien=phim.DaoDien;
            p.DienVien=phim.DienVien;
            p.QuocGia=phim.QuocGia;
            p.NamSX=phim.NamSX;
            p.ThoiLuong=phim.ThoiLuong;
            p.LuotXem=phim.LuotXem;
            p.SoLuongVote=phim.SoLuongVote;
            p.SoSao=phim.SoSao;
            p.SoLuongComment=phim.SoLuongComment;
            p.MaChatLuong=phim.MaChatLuong;
            p.LoaiPhim=phim.LoaiPhim;
            if(p.LoaiPhim==2)
            {
                p.Link = null;
            }
            try
            {
                // Xử lý file ảnh
                if (hinhAnhFile != null && hinhAnhFile.ContentLength > 0)
                {
                    var imagePath = Path.Combine(Server.MapPath("~/Content/img/film"), Path.GetFileName(hinhAnhFile.FileName));
                    hinhAnhFile.SaveAs(imagePath);
                    p.HinhAnh = hinhAnhFile.FileName;
                }

                // Xử lý file video
                //if (videoFile != null && videoFile.ContentLength > 0)
                //{
                //    var videoPath = Path.Combine(Server.MapPath("~/Content/video"), Path.GetFileName(videoFile.FileName));
                //    videoFile.SaveAs(videoPath);
                //    p.Link = videoFile.FileName;
                //}
                // Lưu thay đổi
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                if (selectedTheLoais != null)
                {
                    // Xóa tất cả thể loại cũ của phim
                    var existingTheLoais = db.TheLoai_Phims.Where(tp => tp.MaPhim == phim.MaPhim).ToList();
                    db.TheLoai_Phims.RemoveRange(existingTheLoais);
                    db.SaveChanges();
                    // Thêm thể loại mới
                    foreach (var theLoaiId in selectedTheLoais)
                    {
                        var theLoaiPhim = new TheLoai_Phim { MaPhim = phim.MaPhim, MaTheLoai = theLoaiId };
                        db.TheLoai_Phims.Add(theLoaiPhim);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.MaChatLuong = new SelectList(db.ChatLuongs, "MaChatLuong", "Ten", phim.MaChatLuong);
                ViewBag.Phans = db.Phans.Where(t => t.MaPhim == phim.MaPhim && t.isDel == false).ToList();
                ViewBag.Users = db.Users.ToList();
                ViewBag.TheLoais = db.TheLoais.ToList();
                return View(phim);
            }
        }

        // GET: Admin/Phims/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Phim = "active";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phim phim = db.Phims.Find(id);
            if (phim == null)
            {
                return HttpNotFound();
            }
            // Xóa tất cả thể loại của phim
            //var TheLoaisPhim = db.TheLoai_Phims.Where(tp => tp.MaPhim == id).ToList();
            //db.TheLoai_Phims.RemoveRange(TheLoaisPhim);

            // Xóa tất cả comment của phim
            //var CommentsPhim = db.Comments.Where(tp => tp.MaPhim == id).ToList();
            //db.Comments.RemoveRange(CommentsPhim);

            // Xóa tất cả vote của phim
            //var VotesPhim = db.Votes.Where(tp => tp.MaPhim == id).ToList();
            //db.Votes.RemoveRange(VotesPhim);

            // Xóa tất cả yêu thích của phim
            //var YeuThichPhim = db.YeuThichs.Where(tp => tp.MaPhim == id).ToList();
            //db.YeuThichs.RemoveRange(YeuThichPhim);

            // Xóa tất cả lịch sử của phim
            //var LichSuXemPhim = db.LichSuXems.Where(tp => tp.MaPhim == id).ToList();
            //db.LichSuXems.RemoveRange(LichSuXemPhim);

            //db.SaveChanges();

            //db.Phims.Remove(phim);
            phim.isDel = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Admin/Phims/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ViewBag.Phim = "active";

        //    Phim phim = db.Phims.Find(id);
        //    db.Phims.Remove(phim);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}
