using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Quyen")]
    public class Quyen
    {
        [Key]
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public virtual ICollection<Quyen_User> Quyen_Users { get; set; }
    }
}