using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("TheLoai_Phim")]
    public class TheLoai_Phim
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Phim")]
        public int MaPhim { get; set; }
        [Required]
        [ForeignKey("TheLoai")]
        public int MaTheLoai { get; set; }
        public virtual Phim Phim { get; set; }
        public virtual TheLoai TheLoai { get; set; }
    }
    
}