using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Phim")]
    public class Phim
    {
        [Key]
        public int MaPhim { get; set; }
        [Required]
        public string Ten { get; set; }
        public string Link { get; set; }
        public string NoiDung { get; set; }
        public string HinhAnh { get; set; }
        [Required]
        public string DaoDien { get; set; }
        [Required]
        public string DienVien { get; set; }
        [Required]
        public string QuocGia { get; set; }
        [Required]
        public int NamSX { get; set; }
        [Required]
        public int ThoiLuong { get; set; }
        public int LuotXem { get; set; }
        public int SoLuongVote { get; set; }
        public float SoSao { get; set; }
        public int SoLuongComment { get; set; }
        public int LoaiPhim { get; set; }
        public int MaUserTao { get; set; }
        public DateTime CreateAt{ get; set; }
        public int MaUserSua { get; set; }
        public DateTime UpdateAt{ get; set; }
        public bool isDel { get; set; }

        [Required]
        [ForeignKey("ChatLuong")]
        public int MaChatLuong { get; set; }
        public virtual ChatLuong ChatLuong { get; set; }
        public virtual ICollection<Comment> comments { get; set; }
        public virtual ICollection<Vote> votes { get; set; }
        public virtual ICollection<YeuThich> yeuThichs { get; set; }
        public virtual ICollection<LichSuXem> lichSuXems { get; set; }
        public virtual ICollection<TheLoai_Phim> TheLoai_Phims { get; set; }
        public virtual ICollection<Phan> Phans { get; set; }
    }
}