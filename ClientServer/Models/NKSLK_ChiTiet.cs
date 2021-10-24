namespace ClientServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NKSLK_ChiTiet
    {
        public int? maNKSLK { get; set; }

        public int? maNhanCong { get; set; }

        public TimeSpan? gioBatDau { get; set; }

        public TimeSpan? gioKetThuc { get; set; }

        [Key]
        public int maChiTiet { get; set; }

        public virtual NhanCong NhanCong { get; set; }

        public virtual NKSLK NKSLK { get; set; }
    }
}
