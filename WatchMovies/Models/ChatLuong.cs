using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("ChatLuong")]
    public class ChatLuong
    {
        [Key]
        public int MaChatLuong { get; set; }
        [Required]
        public string Ten { get; set; }
        public virtual ICollection<Phim> phims { get; set; }
    }
}