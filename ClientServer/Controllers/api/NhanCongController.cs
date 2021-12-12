using ClientServer.Models;
using ClientServer.Models.QModel;
using ClientServer.Models.QModel.CongViec;
using ClientServer.Models.QModel.Response;
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
        public async Task<IEnumerable<SLKSelect>> GetWorkInMonth(int id, string date)
        {
            SqlParameter idParams = new SqlParameter("@MaNhanCong", id);
            SqlParameter dateParams = new SqlParameter("@ThangLamViec", date);
            string query = "EXEC NhatKySanLuongKhoan_TheoThang_Update @MaNhanCong, @ThangLamViec";

            var list = await context.Database.SqlQuery<SLKSelect>(query, idParams, dateParams).ToListAsync();
            return list;
        }

        [HttpGet]
        [Route("api/nhancong/tuan")]
        public async Task<IEnumerable<SLKSelect>> GetWorkInWeek(int id, string date)
        {
            SqlParameter idParams = new SqlParameter("@MaNhanCong", id);
            SqlParameter dateParams = new SqlParameter("@ThangLamViec", date);
            string query = "EXEC NhatKySanLuongKhoan_Tuan_Update @MaNhanCong, @ThangLamViec";

            var list = await context.Database.SqlQuery<SLKSelect>(query, idParams, dateParams).ToListAsync();
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

        [HttpGet]
        [Route("api/nhancong/age")]
        public async Task<Reponse> GetNC_Age(int? start = 30, int? end = 45)
        {
            Reponse res = new Reponse();
            try
            {
                string query = @"SELECT maNhanCong, hoTen, ngaySinh, queQuan, DATEDIFF(DAY, NhanCong.ngaySinh, GETDATE()) / 365 AS Tuoi FROM NhanCong
                    WHERE DATEDIFF(DAY, ngaySinh, GETDATE()) 
                    BETWEEN " + start + " * 365 AND " + end + " * 365";

                var list = await context.Database.SqlQuery<NhanCongRes>(query).ToListAsync();

                res.Data = list;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Data = ex;
                res.ErrorCode = 400;
                res.Message = "Lỗi";
                res.Success = false;
            }
            return res;
        }

        [HttpGet]
        [Route("api/nhancong/retired")]
        public async Task<Reponse> GetNC_Retired(string gender = "nam")
        {
            Reponse res = new Reponse();
            try
            {
                int tuoi = 54;
                if (gender.ToLower().Equals("nam")) tuoi = 54;
                else if (gender.ToLower().Equals("nu")) tuoi = 49;

                string query = @"WITH NKSLK_NHANCONG_TUOI(maNhanCong, hoTen, gioiTinh, Tuoi) AS (
                            SELECT maNhanCong, hoTen, gioiTinh,
                            DATEDIFF(year, NhanCong.ngaySinh, GETDATE()) AS Tuoi
                            FROM NhanCong
                        )
                        SELECT * FROM NhanCong JOIN NKSLK_NHANCONG_TUOI
                        ON NhanCong.maNhanCong = NKSLK_NHANCONG_TUOI.maNhanCong
                        WHERE NhanCong.gioiTinh = N'" + gender + @"'
                        AND NKSLK_NHANCONG_TUOI.Tuoi + 1 = " + tuoi;

                var list = await context.Database
                    .SqlQuery<NhanCong>(query)
                    .ToListAsync();

                res.Data = list;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Data = ex;
                res.ErrorCode = 400;
                res.Message = "Lỗi";
                res.Success = false;
            }
            return res;
        }

        [HttpGet]
        [Route("api/nhancong/shift")]
        public async Task<Reponse> GetNC_Shift(int caLam = 3)
        {
            Reponse res = new Reponse();
            try
            {
                string gioBatDau = "22:00:00";
                string gioKetThuc = "6:00:00";

                switch (caLam)
                {
                    case 1:
                        gioBatDau = "6:00:00";
                        gioKetThuc = "14:00:00"; ;
                        break;
                    case 2:
                        gioBatDau = "14:00:00";
                        gioKetThuc = "22:00:00";
                        break;
                    case 3:
                        gioBatDau = "22:00:00";
                        gioKetThuc = "6:00:00";
                        break;
                }

                string query = "SELECT nc.maNhanCong, nc.hoTen, nc.ngaySinh, nc.queQuan " +
                    "FROM NhanCong nc, NKSLK_ChiTiet ct, NKSLK nk WHERE nc.maNhanCong = ct.maNhanCong AND nk.maNKSLK = ct.maNKSLK AND CONVERT(TIME, ct.gioBatDau) >= CONVERT(TIME, '" + gioBatDau + "') AND CONVERT(TIME, ct.gioKetThuc) <= CONVERT(TIME, '" + gioKetThuc + "') GROUP BY nc.maNhanCong, nc.hoTen, nc.ngaySinh, nc.queQuan";

                var list = await context.Database
                    .SqlQuery<NhanCongRes>(query)
                    .ToListAsync();

                res.Data = list;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Data = ex;
                res.ErrorCode = 400;
                res.Message = "Lỗi";
                res.Success = false;
            }
            return res;
        }
    }
}