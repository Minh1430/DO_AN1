using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("TheLoai")]
    public class TheLoai
    {
        [Key]
        public int MaTheLoai { get; set; }
        [Required]
        public string Ten { get; set; }
        public virtual ICollection<TheLoai_Phim> TheLoai_Phims { get; set; }
    }
}