using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ClientServer.Models
{
    public partial class NKSLKDbContext : DbContext
    {
        public NKSLKDbContext()
            : base("name=NKSLKDbContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<CongViec> CongViecs { get; set; }
        public virtual DbSet<DanhMucKhoan_ChiTiet> DanhMucKhoan_ChiTiet { get; set; }
        public virtual DbSet<NhanCong> NhanCongs { get; set; }
        public virtual DbSet<NKSLK> NKSLKs { get; set; }
        public virtual DbSet<NKSLK_ChiTiet> NKSLK_ChiTiet { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhMucKhoan_ChiTiet>()
                .Property(e => e.soLoSanPham)
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.soDangKy)
                .IsUnicode(false);
        }
    }
}
