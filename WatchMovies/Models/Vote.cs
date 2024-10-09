using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Vote")]
    public class Vote
    {
        [Key]
        public int MaVote { get; set; }
        [Required]
        public float SoSao { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGian { get; set; }

        [Required]
        [ForeignKey("User")]
        public int MaUser { get; set; }
        [Required]
        [ForeignKey("Phim")]
        public int MaPhim { get; set; }
        public virtual User User { get; set; }
        public virtual Phim Phim { get; set; }
    }
}