using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Quyen_User")]
    public class Quyen_User
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [ForeignKey("User")]
        public int MaUser { get; set; }
        [Required]
        [ForeignKey("Quyen")]
        public int MaQuyen { get; set; }
        public virtual User User { get; set; }
        public virtual Quyen Quyen { get; set; }
    }
}