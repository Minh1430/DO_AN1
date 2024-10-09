using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("ThongBao")]
    public class ThongBao
    {
        [Key]
        public int MaThongBao{ get; set; }
        public int MaUser{ get; set; }
        public string NoiDung{ get; set; }
        public string Link{ get; set; }
        public DateTime CreateAt{ get; set; }
        public bool DaXem{ get; set; }

        public virtual User User { get; set; }
    }
}