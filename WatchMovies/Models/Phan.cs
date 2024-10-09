using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Phan")]
    public class Phan
    {
        [Key]
        public int MaPhan { get; set; }
        [Required]
        public int MaPhim{ get; set; }
        public string TenPhan { get; set; }
        public int MaUserTao { get; set; }
        public int MaUserSua { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool isDel {  get; set; }
        public virtual Phim Phim { get; set; }
        public virtual ICollection<Tap> Taps { get; set; }
    }
}