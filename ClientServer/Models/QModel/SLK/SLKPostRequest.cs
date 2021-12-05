using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientServer.Models.QModel.SLK
{
    public class SLKAddRequest
    {
        public int maNhanCong { get; set; }
        public int maCongViec { get; set; }
        public int maSanPham { get; set; }
        public int caLam { get; set; }
        public double sanLuongThucTe { get; set; }
        public string soLoSanPham { get; set; }
    }
}