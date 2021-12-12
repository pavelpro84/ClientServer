namespace ClientServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            NKSLK_ChiTiet = new HashSet<NKSLK_ChiTiet>();
        }

        [StringLength(50)]
        public string tenSanPham { get; set; }

        [StringLength(20)]
        public string soDangKy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? hanSuDung { get; set; }

        [StringLength(20)]
        public string quyCach { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngayDangKy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ngaySanXuat { get; set; }

        [Key]
        public int maSanPham { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NKSLK_ChiTiet> NKSLK_ChiTiet { get; set; }
    }
}
