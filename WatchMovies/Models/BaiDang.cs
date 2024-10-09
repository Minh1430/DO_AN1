using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("BaiDang")]
    public class BaiDang
    {
        [Key]
        public int MaBaiDang { get; set; }
        [Required]
        public string Ten { get; set; }
        [Required]
        public string NoiDung { get; set; }
        public string HinhAnh { get; set; }
        public string DaoDien { get; set; }
        public string DienVien { get; set; }

        [Required]
        [ForeignKey("UserTao")]
        public int MaUserTao { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual User UserTao { get; set; }

        //[Required]
        //[ForeignKey("UserSua")]
        //public int MaUserSua { get; set; }
        //public DateTime NgaySua { get; set; }
        //public virtual User UserSua { get; set; }

    }
}