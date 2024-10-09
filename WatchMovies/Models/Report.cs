using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    [Table("Report")]
    public class Report
    {
        [Key]
        public int MaReport { get; set; }
        [Required]
        [ForeignKey("User")]
        public int MaUser { get; set; }
        [Required]
        [ForeignKey("Comment")]
        public int MaComment { get; set; }
        public DateTime NgayTao { get; set; }
        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}