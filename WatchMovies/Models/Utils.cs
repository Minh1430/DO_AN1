using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    public static class Utils
    {
        public static void DisConnect()
        {
            WatchMoviesContext db = new WatchMoviesContext();
            db.Dispose();
        }
        public static int GetThongBao(int MaUser)
        {
            WatchMoviesContext db = new WatchMoviesContext();
            int ThongBao = db.Users.Find(MaUser).ThongBaos.Where(t=>t.DaXem==false).Count();
            db.Dispose();

            return ThongBao;
        }
        
        public static User GetUser(int MaUser)
        {
            WatchMoviesContext db = new WatchMoviesContext();
            User user = db.Users.Find(MaUser);
            db.Dispose();

            return user;
        }
    }
}