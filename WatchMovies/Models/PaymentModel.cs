using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WatchMovies.Models
{
    public class PaymentModel
    {
        public int MaThanhToan { get; set; }
        public int MaUser { get; set; }
        public int MaPhim { get; set; }
        public string TenPhim { get; set; }
        public decimal GiaThanhToan { get; set; }
        public DateTime NgayThanhToan { get; set; }
    }
}