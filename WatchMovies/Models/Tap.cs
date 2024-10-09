using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Tap")]
    public class Tap
    {
        [Key]
        public int MaTap { get; set; }
        [Required]
        public int MaPhan { get; set; }
        public string Ten { get; set; }
        public string Link{ get; set; }
        public int MaUserTao { get; set; }
        public DateTime CreateAt { get; set; }
        public int MaUserSua { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool isDel { get; set; }
        public virtual Phan Phan { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}