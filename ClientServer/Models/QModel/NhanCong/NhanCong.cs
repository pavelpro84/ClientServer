using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientServer.Models.QModel.CongViec
{
    public class NhanCongRes
    {
        public int maNhanCong { get; set; }
        public string hoTen { get; set; }
        public DateTime? ngaySinh { get; set; }
        public string? queQuan { get; set; }
        public int Tuoi { get; set; }
    }
}