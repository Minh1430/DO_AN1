using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int MaUser { get; set; }
        [Required]
        public string Ten { get; set; }
        [Required]
        public string Email { get; set; }
        public string SDT { get; set; }
        public string HinhAnh { get; set; }
        [Required]
        public string Password { get; set; }
        public bool isDel { get; set; }
        public virtual ICollection<YeuThich> yeuThichs { get; set; }
        public virtual ICollection<LichSuXem> lichSuXems { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
        public virtual ICollection<Vote> votes { get; set; }
        public virtual ICollection<Report> reports { get; set; }
        //[ForeignKey("TaoBaiDang")]
        public virtual ICollection<BaiDang> BaiDangsTao { get; set; }
        public virtual ICollection<Quyen_User> Quyen_Users { get; set; }
        public virtual ICollection<ThongBao> ThongBaos { get; set; }
    }
}