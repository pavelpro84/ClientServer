using ClientServer.Models;
using ClientServer.Models.QModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClientServer.Controllers.api
{
    public class NhanCongController : ApiController
    {
        private NKSLKDbContext context = new NKSLKDbContext();

        [HttpGet]
        [Route("api/nhancong")]
        public async Task<PagingData> GetAll(
            int? pageIndex = 0,
            int? pageSize = 10,
            string search = "",
            string sort = "id_asc")
        {
            var searchStr = SlugGenerator.SlugGenerator.GenerateSlug(search);
            var list = await context.NhanCongs.AsNoTracking().ToListAsync();
            if (!String.IsNullOrEmpty(searchStr))
            {
                list = list.Where(x =>
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.hoTen)
                    .Contains(searchStr))
                    .ToList();
            }
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.hoTen).ToList();
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.hoTen).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maNhanCong).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maNhanCong).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maNhanCong).ToList();
                    break;
            }

            var pagingData = new PagingData();
            pagingData.TotalRecord = list.Count;
            pagingData.Data = list
                .Skip(pageIndex.Value * pageSize.Value)
                .Take(pageSize.Value)
                .ToList();

            return pagingData;
        }

        [HttpGet]
        [Route("api/nhancong/{id}")]
        public async Task<NhanCong> GetById(int id)
        {
            return await context.NhanCongs.FindAsync(id);
        }

        [HttpGet]
        [Route("api/nhancong/thang")]
        public async Task<IEnumerable<SLK>> GetWorkInMonth(int id, string date)
        {
            SqlParameter idParams = new SqlParameter("@MaNhanCong", id);
            SqlParameter dateParams = new SqlParameter("@ThangLamViec", date);
            string query = "EXEC NhatKySanLuongKhoan_TheoThang_Update @MaNhanCong, @ThangLamViec";

            var list = await context.Database.SqlQuery<SLK>(query, idParams, dateParams).ToListAsync();
            return list;
        }

        [HttpGet]
        [Route("api/nhancong/tuan")]
        public async Task<IEnumerable<SLK>> GetWorkInWeek(int id, string date)
        {
            SqlParameter idParams = new SqlParameter("@MaNhanCong", id);
            SqlParameter dateParams = new SqlParameter("@ThangLamViec", date);
            string query = "EXEC NhatKySanLuongKhoan_Tuan_Update @MaNhanCong, @ThangLamViec";

            var list = await context.Database.SqlQuery<SLK>(query, idParams, dateParams).ToListAsync();
            return list;
        }

        // them 1 phan tu
        [HttpPost]
        [Route("api/nhancong/add")]
        public async Task<NhanCong> AddCongViec(NhanCong nhancong)
        {
            context.NhanCongs.Add(nhancong);
            await context.SaveChangesAsync();
            return nhancong;
        }

        //sua 1 phan tu
        [HttpPost]
        [Route("api/nhancong/edit/{id}")]
        public async Task<Boolean> EditNhanCong(NhanCong nhancong, int id)
        {
            NhanCong nhancong_01 = context.NhanCongs.Find(id);
            if (nhancong_01 == null)
            {
                return false;
            }
            nhancong_01.hoTen = nhancong.hoTen;
            nhancong_01.ngaySinh = nhancong.ngaySinh;
            nhancong_01.gioiTinh = nhancong.gioiTinh;
            nhancong_01.phongBan = nhancong.phongBan;
            nhancong_01.chucVu = nhancong.chucVu;
            nhancong_01.chucVu = nhancong.chucVu;
            nhancong_01.queQuan = nhancong.queQuan;
            nhancong_01.luongBaoHiem = nhancong.luongBaoHiem;
            await context.SaveChangesAsync();
            return true;

        }

        // xoa 1 phan tu
        [HttpDelete]
        [Route("api/nhancong/delete/{id}")]
        public async Task<Boolean> DeleteNhanCong(int id)
        {
            NhanCong nhancong = context.NhanCongs.Find(id);
            if (nhancong == null)
            {
                return false;
            }
            context.NhanCongs.Remove(nhancong);
            await context.SaveChangesAsync();
            return true;

        }
    }
}