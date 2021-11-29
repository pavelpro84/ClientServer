using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientServer.Models.QModel
{
    public class SLKSelect
    {
        public int maNKSLK { get; set; }
        public string tenCongViec { get; set; }
        public DateTime ngay { get; set; }
        public string hoTen { get; set; }
        public TimeSpan gioBatDau { get; set; }
        public TimeSpan gioKetThuc { get; set; }
    }
}