using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WatchMovies.Models;

namespace WatchMovies.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private WatchMoviesContext db = new WatchMoviesContext();
        // GET: Admin/Dashboard
        public ActionResult Index(int? page)
        {
            ViewBag.Dashboard = "active";
            var list = db.Phims.Where(p=>p.isDel==false).OrderByDescending(p=>p.MaPhim).ToList();
            ViewBag.SoLuongPhim = list.Count();
            ViewBag.SoLuongComment = db.Comments.ToList().Count();
            ViewBag.SoLuongVote = db.Votes.ToList().Count();
            var x = db.Phims.ToList();
            double tong = 0;
            foreach(var item in x)
            {
                tong += item.SoSao;
            }
            ViewBag.SoSaoTB = Math.Round((tong / ViewBag.SoLuongPhim), 1);
            int MaUser = int.Parse(Session["Ma"].ToString());
            var quyen = db.Quyen_Users.Where(e=>e.MaUser == MaUser).Select(p=>p.MaQuyen).ToList();

            Session["QLPhim"] = false;
            Session["QLBaiDang"] = false;
            Session["QLChatLuong"] = false;
            Session["QLComment"] = false;
            Session["QLTheLoai"] = false;
            Session["QLUser"] = false;
            foreach (var item in quyen)
            {
                if (item == 1)
                {
                    Session["QLPhim"] = true;
                    Session["QLBaiDang"] = true;
                    Session["QLChatLuong"] = true;
                    Session["QLComment"] = true;
                    Session["QLTheLoai"] = true;
                    Session["QLUser"] = true;

                    break;
                }else if (item == 3) { Session["QLPhim"] = true; }
                else if (item == 4) { Session["QLBaiDang"] = true; }
                else if (item == 5) { Session["QLChatLuong"] = true; }
                else if (item == 6) { Session["QLComment"] = true;}
                else if (item == 7) { Session["QLTheLoai"] = true; }
                
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
    }
}