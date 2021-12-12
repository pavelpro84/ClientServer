namespace ClientServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CongViec")]
    public partial class CongViec
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CongViec()
        {
            NKSLK_ChiTiet = new HashSet<NKSLK_ChiTiet>();
        }

        [StringLength(50)]
        public string tenCongViec { get; set; }

        public double? dinhMucKhoan { get; set; }

        [StringLength(20)]
        public string donViKhoan { get; set; }

        public double? heSoKhoan { get; set; }

        public double? dinhMucLaoDong { get; set; }

        public double? donGia { get; set; }

        [Key]
        public int maCongViec { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NKSLK_ChiTiet> NKSLK_ChiTiet { get; set; }
    }
}
