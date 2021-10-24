namespace ClientServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DanhMucKhoan_ChiTiet
    {
        public int? maNKSLK { get; set; }

        public int? maCongViec { get; set; }

        public double? sanLuongThucTe { get; set; }

        [StringLength(20)]
        public string soLoSanPham { get; set; }

        public int? maSanPham { get; set; }

        [Key]
        public int maChiTiet { get; set; }

        public virtual CongViec CongViec { get; set; }

        public virtual NKSLK NKSLK { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
