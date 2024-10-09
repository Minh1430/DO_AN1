using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using WatchMovies.Models;
using Diacritics;
using Diacritics.Extensions;
using PagedList;
using System.Web.UI;

namespace WatchMovies.Controllers
{
    public class HomeController : Controller
    {
        protected WatchMoviesContext db = new WatchMoviesContext();
        //lấy ra danh sách thể loại
        [ChildActionOnly]
        public PartialViewResult MenuTheLoai()
        {
            var list = db.TheLoais.ToList();

            return PartialView(list);
        }
        //lấy ra danh sách thông báo
        [ChildActionOnly]
        public PartialViewResult ThongBaoList()
        {
            if (Session["Ma"] == null)
                return PartialView();
            else
            {
                int MaUser = int.Parse(Session["Ma"].ToString());
                var user = db.Users.Find(MaUser);
                if (user != null)
                {
                    var list = user.ThongBaos.OrderByDescending(t=>t.CreateAt).ToList();
                    return PartialView(list);
                }
                else
                {
                    return PartialView(); 
                }
            }   
        }

        //hiển thị danh sách phim theo thể loại
        [Route("TheLoai/{MaTheLoai}")]
        public ActionResult HienThiTheoTheLoai(string MaTheLoai, int? page)
        {
            var list = db.Phims.Where(e => e.isDel == false && e.TheLoai_Phims.Any(t=>t.MaTheLoai.ToString() == MaTheLoai)).OrderByDescending(e=>e.MaPhim);
            TheLoai x = db.TheLoais.Where(t=>t.MaTheLoai.ToString() == MaTheLoai).FirstOrDefault();
            ViewBag.ListQuality = db.ChatLuongs.ToList();

            ViewBag.TenTheLoai = x.Ten;
            ViewBag.MaTheLoai = MaTheLoai;
            ViewBag.sortedBy = null;

            int pageSize = 18;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber,pageSize));
        }

        //Lọc phim
        [Route("TheLoai")]
        public ActionResult HienThiTheoTheLoai(string MaTheLoai, string sortBy, string quality, string star, string year, int? page)
        {
            var list = db.Phims.Where(e => e.isDel == false && e.TheLoai_Phims.Any(t => t.MaTheLoai.ToString() == MaTheLoai)).ToList();
            TheLoai x = db.TheLoais.Where(t => t.MaTheLoai.ToString() == MaTheLoai).FirstOrDefault();
            ViewBag.ListQuality = db.ChatLuongs.ToList();

            ViewBag.TenTheLoai = x.Ten;
            ViewBag.MaTheLoai = MaTheLoai;
            ViewBag.sortedBy = null;

            if (!string.IsNullOrEmpty(year))
            {
                for(int item = 0; item<list.Count;item++)
                {
                    if (list[item].NamSX.ToString() != year)
                    {
                        list.RemoveAt(item);
                        item--;
                    }
                }
            }
            if (!string.IsNullOrEmpty(quality))
            {
                for (int item = 0; item < list.Count; item++)
                {
                    if (list[item].ChatLuong.Ten != quality)
                    {
                        list.RemoveAt(item);
                        item--;
                    }
                }
            }
            if (!string.IsNullOrEmpty(star))
            {
                for (int item = 0; item < list.Count; item++)
                {
                    if (list[item].SoSao < int.Parse(star))
                    {
                        list.RemoveAt(item);
                        item--;
                    }
                }
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Ngày đăng")
                {
                    list = list.OrderByDescending(m => m.MaPhim).ToList();
                }
                else if (sortBy == "Năm sản xuất")
                {
                    list = list.OrderByDescending(m => m.NamSX).ToList();
                }
                else if (sortBy == "Lượt xem")
                {
                    list = list.OrderByDescending(m => m.LuotXem).ToList();
                }
            }
            int pageSize = 18;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        //hiển thị danh sách yêu thích
        public ActionResult XemDanhSachYeuThich()
        {
            //chưa đăng nhập sẽ chuyển đến trang login
            if (Session["Ma"] == null)
                return RedirectToAction("Login", "Login");
            else
            {
                int mauser = int.Parse(Session["Ma"].ToString());
                var list = db.YeuThichs.Where(t => t.MaUser == mauser).OrderByDescending(a => a.MaYeuThich).ToList();
                List<Phim> ds = new List<Phim>();
                foreach (var t in list)
                {
                    if (t.Phim.isDel == false)
                    {
                        ds.Add(t.Phim);
                    }
                }

                return View(ds);
            }
        }

        //hiển thị lịch sử xem
        public ActionResult XemLichSuXem()
        {
            //chưa đăng nhập sẽ chuyển đến trang login
            if (Session["Ma"] == null)
                return RedirectToAction("Login", "Login");
            else
            {
                int mauser = int.Parse(Session["Ma"].ToString());
                var list = db.LichSuXems.Where(t => t.MaUser == mauser).OrderByDescending(a => a.MaLichSu).ToList();
                List<Phim> ds = new List<Phim>();
                foreach (var t in list)
                {
                    if (t.Phim.isDel == false)
                    {
                        ds.Add(t.Phim);
                    }
                }

                return View(ds);
            }
        }

        //trang chủ
        public ActionResult Index()
        {
            var hot = db.Phims.Where(t => t.isDel == false).OrderByDescending(p=>p.LuotXem).Take(10).ToList();
            var list = db.Phims.Where(t => t.isDel == false).OrderByDescending(p => p.NamSX).Take(18).ToList();
            ViewBag.hot = hot;
            return View(list);
        }

        //tìm kiếm phim theo từ khóa
        public ActionResult TimKiem(string search, int? page)
        {
            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                //chuyển từ khóa thành viết thường và xóa dấu
                search = search.ToLower().RemoveDiacritics();
                //tìm kiếm tên, đạo diễn, diễn viên có chứa từ khóa
                var list = db.Phims.AsEnumerable()
                .Where(p => p.Ten.ToLower().RemoveDiacritics().Contains(search) ||
                            p.DaoDien.ToLower().RemoveDiacritics().Contains(search) ||
                            p.DienVien.ToLower().RemoveDiacritics().Contains(search) &&
                            p.isDel == false)
                .ToList();
                int pageSize = 18;
                int pageNumber = (page ?? 1);
                return View(list.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public bool UpdateThongBaoDaXem(int MaUser)
        {
            WatchMoviesContext db = new WatchMoviesContext();
            try
            {
                List<ThongBao> thongBaos = db.Users.Find(MaUser).ThongBaos.Where(t => t.DaXem == false).ToList();
                foreach (ThongBao tb in thongBaos)
                {
                    tb.DaXem = true;
                }
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
      
     
    }
}