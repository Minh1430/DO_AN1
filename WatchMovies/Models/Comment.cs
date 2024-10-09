using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int MaComment { get; set; }
        [Required]
        public string NoiDungComment { get; set; }
        public DateTime ThoiGian { get; set; }

        [Required]
        [ForeignKey("User")]
        public int MaUser { get; set; }
        public int MaPhim { get; set; }
        public int? MaTap { get; set; }
        public int? RepCommentID { get; set; }
        public virtual User User { get; set; }
        public virtual Phim Phim { get; set; }
        public virtual Tap Tap { get; set; }
        public virtual ICollection<Report> reports { get; set; }
    }
}