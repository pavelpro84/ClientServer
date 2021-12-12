namespace ClientServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NKSLK")]
    public partial class NKSLK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NKSLK()
        {
            NKSLK_ChiTiet = new HashSet<NKSLK_ChiTiet>();
        }

        [Column(TypeName = "date")]
        public DateTime? ngay { get; set; }

        [Key]
        public int maNKSLK { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NKSLK_ChiTiet> NKSLK_ChiTiet { get; set; }
    }
}
