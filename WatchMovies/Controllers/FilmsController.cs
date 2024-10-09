using Antlr.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;

namespace WatchMovies.Controllers
{
    public class FilmsController : Controller
    {
        WatchMoviesContext db = new WatchMoviesContext();
        
        //khới tạo database
        public ActionResult Index()
        {
            ViewBag.sl = db.Phims.Count();
            return View();
        }

        //xem danh sách bài đăng
        public ActionResult XemBaiDang()
        {
            var list = db.BaiDangs.ToList();
            return View(list);
        }

        //xem chi tiết một bài đăng
        public ActionResult XemChiTietBaiDang(int id)
        {
            var x = db.BaiDangs.Where(e=>e.MaBaiDang == id).FirstOrDefault();
            return View(x);
        }

        //báo cáo một comment
        public ActionResult ThemReport(int MaComment)
        {
            Report report = new Report();
            report.MaComment = MaComment;
            report.MaUser = int.Parse(Session["Ma"].ToString());
            report.NgayTao = DateTime.Now;
            
            db.Reports.Add(report);
            db.SaveChanges();

            var x = db.Comments.Find(MaComment);
            return RedirectToAction("Details", "Films", new { id = x.MaPhim });
        }

        //xem chi tiết một phim
        public ActionResult Details(int? id, int? MaTap)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phim p = db.Phims.Find(id);
            if(p.isDel==true)
            {
                return HttpNotFound();
            }
            //lấy danh sách comment cha của phim
            ViewBag.CommentList = db.Phims.Where(e=>e.MaPhim == id)
                                .SelectMany(c=>c.comments)
                                .Where(c=>c.RepCommentID == null || c.RepCommentID == 0)
                                .OrderByDescending(t=>t.ThoiGian)
                                .ToList();
            //lấy danh sách RepComment của phim
            ViewBag.RepCommentList = db.Phims.Where(e => e.MaPhim == id)
                                .SelectMany(c => c.comments)
                                .Where(c => c.RepCommentID != null && c.RepCommentID > 0)
                                .OrderBy(t => t.ThoiGian)
                                .ToList();
            //lấy danh sách vote của phim
            ViewBag.VoteList = db.Phims.Where(e => e.MaPhim == id)
                                .SelectMany(c => c.votes)
                                .OrderByDescending(t => t.ThoiGian)
                                .ToList();
            //danh sách 6 phim cùng thể loại có lượt xem cao nhất
            var danhSachTheLoaiCuaPhim = p.TheLoai_Phims.Select(e => e.MaTheLoai).ToList();
            ViewBag.GoiY=db.Phims.Where(e=>e.TheLoai_Phims.Any
            (t=>danhSachTheLoaiCuaPhim.Contains(t.MaTheLoai)))
                .OrderByDescending(e=>e.LuotXem)
                .Take(6)
                .ToList();
            //kiểm tra phim này đã thêm vào danh sách yêu thích của người dùng hay chưa
            ViewBag.DaThemVaoYeuThich = true;
            //Danh sách phần của phim
            ViewBag.Phan = db.Phans.Where(e=>e.MaPhim==id && e.isDel==false).ToList();
            if (Session["Ma"]==null)
                ViewBag.mauser = true;
            else
            {
                ViewBag.mauser = false;
                string mauser = Session["Ma"].ToString();
                var x = db.YeuThichs.Where(e => e.MaUser.ToString() == mauser && e.MaPhim==id).FirstOrDefault();
                if(x != null) { ViewBag.DaThemVaoYeuThich = false; }
            }
            if (p == null)
            {
                return HttpNotFound();
            }
            if(MaTap > 0)
            {
                ViewBag.Tap = db.Taps.Find(MaTap);
            }
            return View(p);
        }

        //thêm comment cho phim
        [HttpPost]
        public ActionResult CommentAjax(string MaPhim, string MaTap, string RepCommentID, string NoiDung)
        {
            //khởi tạo một comment mới
            int TapID = int.Parse(MaTap);
            int ParentCommentID = int.Parse(RepCommentID);
            Comment x = new Comment();
            x.MaUser = int.Parse(Session["Ma"].ToString());
            x.NoiDungComment = NoiDung;
            x.ThoiGian = DateTime.Now;
            x.MaPhim = int.Parse(MaPhim);
            bool isRepComment = (x.NoiDungComment.ToArray()[0] == '@');
            if(ParentCommentID > 0 && isRepComment)
            {
                x.RepCommentID = ParentCommentID;
            }
            Phim phim = db.Phims.Where(p => p.MaPhim.ToString() == MaPhim).FirstOrDefault();

            try
            {
                //thêm comment và tăng số lượng coment của phim thêm 1
                db.Comments.Add(x);
                db.Entry(x).Reference(c => c.User).Load();
                if(TapID > 0)
                {
                    x.MaTap = 1;
                }
                phim.SoLuongComment++;
                db.Entry(phim).State = EntityState.Modified;
                db.SaveChanges();

                if (x.RepCommentID <= 0 || x.RepCommentID == null)
                    return PartialView(x);
                else
                {
                    return PartialView("RepCommentAjax",x);
                }
            }
            catch
            {
                return HttpNotFound();
            }

        }

        //thêm vote cho phim
        [HttpPost]
        public ActionResult VoteAjax(string NoiDung, string SoSao, string MaPhim)
        {
            //khởi tạo vote mới
            Vote x = new Vote();
            x.SoSao = float.Parse(SoSao) / 10;
            x.NoiDung = NoiDung;
            x.ThoiGian = DateTime.Now;
            x.MaUser = int.Parse(Session["Ma"].ToString());
            x.MaPhim = int.Parse(MaPhim);

            Phim phim = db.Phims.Where(t => t.MaPhim.ToString() == MaPhim).FirstOrDefault();

            try
            {
                //thêm vote và tính lại số sao của phim
                phim.SoLuongVote += 1;
                var list = phim.votes.ToList();
                float tong = x.SoSao;
                foreach (var v in list)
                {
                    tong = tong + v.SoSao;
                }
                double sao = Math.Round((tong / phim.SoLuongVote), 1);
                phim.SoSao = (float)sao;
                db.Entry(phim).State = EntityState.Modified;

                db.Votes.Add(x);
                db.Entry(x).Reference(c => c.User).Load();

                db.SaveChanges();

                return PartialView(x);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        //tăng lượt xem cho phim và thêm phim vào lịch sử xem
        [HttpPost]
        public ActionResult XemPhim(string MaPhim)
        {
            int id = int.Parse(MaPhim);
            Phim p = db.Phims.Find(id);
            if (p != null)
            {
                //tăng lượt xem và lưu vào lịch sử xem
                p.LuotXem++;
                if (Session["Ma"] != null)
                {
                    // thêm phim vào lịch sử xem nếu đã đăng nhập
                    int mauser = int.Parse(Session["Ma"].ToString());
                    LichSuXem newitem = new LichSuXem();
                    newitem.MaPhim = int.Parse(MaPhim);
                    newitem.MaUser = mauser;
                    var ds = db.LichSuXems.Where(t => t.MaUser == mauser).ToList();
                    //nếu danh sách lịch sử xem của user trống thêm vào lịch sử xem
                    if (ds == null)
                    {
                        db.LichSuXems.Add(newitem);
                    }
                    else
                    {
                        //kiểm tra phim đã có trong lịch sử xem chưa
                        var x = ds.Where(t => t.MaPhim == id).FirstOrDefault();
                        if(x==null)
                            //chưa có thì thêm mới
                            db.LichSuXems.Add(newitem);
                        else
                        {
                            //đã có thì xóa cũ thêm mới
                            db.LichSuXems.Remove(x);
                            db.LichSuXems.Add(newitem);
                        }
                    }
                }
                db.SaveChanges();
                return new EmptyResult();
            }
            else
            {
                return HttpNotFound();
            }
        }

        //thêm phim vào danh sách yêu  thích
        [HttpPost]
        public ActionResult ThemVaoDanhSachYeuThich(string MaPhim)
        {
            //chưa đăng nhập sẽ chuyển đến trang login
            if (Session["Ma"] == null)
                return RedirectToAction("Login", "Login");
            else
            {
                // thêm phim vào yêu thích
                YeuThich yeuThich = new YeuThich();
                yeuThich.MaPhim = int.Parse(MaPhim);
                yeuThich.MaUser = int.Parse(Session["Ma"].ToString());
                db.YeuThichs.Add(yeuThich);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = yeuThich.MaPhim });
            }
        }

        //xóa phim khởi danh sách yêu thích
        [HttpPost]
        public ActionResult XoaKhoiDanhSachYeuThich(string MaPhim)
        {
            int idphim = int.Parse(MaPhim);
            int iduser = int.Parse(Session["Ma"].ToString());
            YeuThich yeuThich = db.YeuThichs.Where(e=>e.MaPhim==idphim && e.MaUser==iduser).FirstOrDefault();
            db.YeuThichs.Remove(yeuThich);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = yeuThich.MaPhim });
        }

        //Lưu time của video trước khi thoát
        [HttpPost]
        public void SaveCurrentTime(int MaPhim, int MaTap, int Time)
        {
            Phim phim = db.Phims.Find(MaPhim);
            //phim lẻ
            if(phim.LoaiPhim == 1)
            {
                Dictionary<int, int> watchedMovies = new Dictionary<int, int>();
                HttpCookie moviesCookie = Request.Cookies["WatchedMovies1"];

                if (moviesCookie != null)
                {
                    // Nếu cookie tồn tại, chuyển đổi giá trị của cookie thành danh sách MaPhim và currentTime
                    string json = moviesCookie.Value;
                    watchedMovies = JsonConvert.DeserializeObject<Dictionary<int, int>>(json);
                }
                if (!watchedMovies.ContainsKey(MaPhim))
                {
                    watchedMovies.Add(MaPhim, Time);
                }
                else
                {
                    watchedMovies[MaPhim] = Time;
                }

                // Chuyển đổi watchedMovies thành chuỗi JSON
                string jsonData = JsonConvert.SerializeObject(watchedMovies);

                // Tạo một cookie mới để lưu watchedMovies
                HttpCookie updatedCookie = new HttpCookie("WatchedMovies1", jsonData);
                Response.Cookies.Add(updatedCookie);
            }
            //phim bộ
            else
            {
                Dictionary<int, int> watchedMovies = new Dictionary<int, int>();
                HttpCookie moviesCookie = Request.Cookies["WatchedMovies2"];

                if (moviesCookie != null)
                {
                    // Nếu cookie tồn tại, chuyển đổi giá trị của cookie thành danh sách MaPhim và currentTime
                    string json = moviesCookie.Value;
                    watchedMovies = JsonConvert.DeserializeObject<Dictionary<int, int>>(json);
                }
                if (!watchedMovies.ContainsKey(MaTap))
                {
                    watchedMovies.Add(MaTap, Time);
                }
                else
                {
                    watchedMovies[MaTap] = Time;
                }

                // Chuyển đổi watchedMovies thành chuỗi JSON
                string jsonData = JsonConvert.SerializeObject(watchedMovies);

                // Tạo một cookie mới để lưu watchedMovies
                HttpCookie updatedCookie = new HttpCookie("WatchedMovies2", jsonData);
                Response.Cookies.Add(updatedCookie);
            }
        }
        public ActionResult qrpay(string MaPhim)
        {
            return View();
        }

        
        public ActionResult Phim_rap(string phuongthucqr)
        {

            return View();
        }
    }
}