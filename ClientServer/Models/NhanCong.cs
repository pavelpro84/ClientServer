namespace ClientServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanCong")]
    public partial class NhanCong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanCong()
        {
            NKSLK_ChiTiet = new HashSet<NKSLK_ChiTiet>();
        }

        [StringLength(50)]
        public string hoTen { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaySinh { get; set; }

        [StringLength(5)]
        public string gioiTinh { get; set; }

        [StringLength(50)]
        public string phongBan { get; set; }

        [StringLength(50)]
        public string chucVu { get; set; }

        [StringLength(50)]
        public string queQuan { get; set; }

        public double? luongBaoHiem { get; set; }

        [Key]
        public int maNhanCong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NKSLK_ChiTiet> NKSLK_ChiTiet { get; set; }
    }
}
