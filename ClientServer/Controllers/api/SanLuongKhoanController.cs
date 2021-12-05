using ClientServer.Models;
using ClientServer.Models.QModel;
using ClientServer.Models.QModel.Response;
using ClientServer.Models.QModel.SLK;
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
    public class SanLuongKhoanController : ApiController
    {
        private NKSLKDbContext context = new NKSLKDbContext();

        [HttpGet]
        [Route("api/slk/all")]
        public async Task<IEnumerable<NKSLK>> Get()
        {
            List<NKSLK> list = await context.NKSLKs.AsNoTracking().ToListAsync();
            return list;
        }

        [HttpGet]
        [Route("api/slk/thang")]
        public async Task<PagingData> GetSLKThang(
            string date,
            int? pageIndex = 0,
            int? pageSize = 10,
            string search = "",
            string sort = "id_asc")
        {
            var searchStr = SlugGenerator.SlugGenerator.GenerateSlug(search);
            SqlParameter dateParams = new SqlParameter("@ThangLamViec", date);
            string query = "EXEC NKSLK_NhaMay @ThangLamViec";

            var list = await context.Database.SqlQuery<SLKSelect>(query, dateParams).ToListAsync();

            if (!String.IsNullOrEmpty(searchStr))
            {
                list = list.Where(x =>
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.tenCongViec)
                    .Contains(searchStr) ||
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.hoTen)
                    .Contains(searchStr)
                ).ToList();
            }
            switch (sort)
            {
                case "work_name_desc":
                    list = list.OrderByDescending(s => s.tenCongViec).ToList();
                    break;
                case "work_name_asc":
                    list = list.OrderBy(s => s.tenCongViec).ToList();
                    break;
                case "empl_name_desc":
                    list = list.OrderByDescending(s => s.hoTen).ToList();
                    break;
                case "empl_name_asc":
                    list = list.OrderBy(s => s.hoTen).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maNKSLK).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maNKSLK).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maNKSLK).ToList();
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
        [Route("api/slk/tuan")]
        public async Task<PagingData> GetSLKTuan(
            string date,
            int? pageIndex = 0,
            int? pageSize = 10,
            string search = "",
            string sort = "id_asc")
        {
            var searchStr = SlugGenerator.SlugGenerator.GenerateSlug(search);
            SqlParameter dateParams = new SqlParameter("@Date", date);
            string query = "EXEC NKSLK_NhaMay_Tuan @Date";

            var list = await context.Database.SqlQuery<SLKSelect>(query, dateParams).ToListAsync();

            if (!String.IsNullOrEmpty(searchStr))
            {
                list = list.Where(x =>
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.tenCongViec)
                    .Contains(searchStr) ||
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.hoTen)
                    .Contains(searchStr)
                ).ToList();
            }
            switch (sort)
            {
                case "work_name_desc":
                    list = list.OrderByDescending(s => s.tenCongViec).ToList();
                    break;
                case "work_name_asc":
                    list = list.OrderBy(s => s.tenCongViec).ToList();
                    break;
                case "empl_name_desc":
                    list = list.OrderByDescending(s => s.hoTen).ToList();
                    break;
                case "empl_name_asc":
                    list = list.OrderBy(s => s.hoTen).ToList();
                    break;
                case "prod_name_desc":
                    list = list.OrderByDescending(s => s.tenSanPham).ToList();
                    break;
                case "prod_name_asc":
                    list = list.OrderBy(s => s.tenSanPham).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maNKSLK).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maNKSLK).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maNKSLK).ToList();
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
        [Route("api/slk/thang_tuan")]
        public async Task<PagingData> GetSLK(
            string date,
            bool? thang = true,
            int? pageIndex = 0,
            int? pageSize = 10,
            string search = "",
            string sort = "id_asc")
        {
            string query = "";
            if (thang.Value)
            {
                query = "EXEC NKSLK_NhaMay @ThangLamViec";
            }
            else
            {
                query = "EXEC NKSLK_NhaMay_Tuan @Date";
            }
            SqlParameter dateParams = new SqlParameter(thang.Value ? "@ThangLamViec" : "@Date", date);
            var list = await context.Database.SqlQuery<SLKSelect>(query, dateParams).ToListAsync();

            var searchStr = SlugGenerator.SlugGenerator.GenerateSlug(search);
            if (!String.IsNullOrEmpty(searchStr))
            {
                list = list.Where(x =>
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.tenCongViec)
                    .Contains(searchStr) ||
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.hoTen)
                    .Contains(searchStr)
                ).ToList();
            }
            switch (sort)
            {
                case "work_name_desc":
                    list = list.OrderByDescending(s => s.tenCongViec).ToList();
                    break;
                case "work_name_asc":
                    list = list.OrderBy(s => s.tenCongViec).ToList();
                    break;
                case "empl_name_desc":
                    list = list.OrderByDescending(s => s.hoTen).ToList();
                    break;
                case "empl_name_asc":
                    list = list.OrderBy(s => s.hoTen).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maNKSLK).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maNKSLK).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maNKSLK).ToList();
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
        [Route("api/slk/day")]
        public async Task<PagingData> GetSLKDay(
            int? pageIndex = 0,
            int? pageSize = 10,
            string sort = "day_desc")
        {
            var list = await context.NKSLKs.ToListAsync();
            switch (sort)
            {
                case "day_desc":
                    list = list.OrderByDescending(s => s.ngay).ToList();
                    break;
                case "day_asc":
                    list = list.OrderBy(s => s.ngay).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maNKSLK).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maNKSLK).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maNKSLK).ToList();
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
        [Route("api/slk/find_day")]
        public async Task<Dictionary<string, object>> FindSLKDay()
        {
            var today = DateTime.Today;
            var item = await context.NKSLKs
                .Where(t => DbFunctions.TruncateTime(t.ngay) == today)
                .FirstOrDefaultAsync();

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("dateTimeNow", today);
            result.Add("item", item);

            return result;
        }

        [HttpDelete]
        [Route("api/slk/delete_day/{id}")]
        public async Task<Reponse> DeleteSLKDay(int id)
        {
            Reponse res = new Reponse();
            try
            {
                var item = await context.NKSLKs.FindAsync(id);
                if (item == null)
                {
                    res.Success = false;
                    res.Message = "Không tìm thấy ngày SLK";
                    res.ErrorCode = 400;
                    return res;
                }

                context.NKSLKs.Remove(item);
                await context.SaveChangesAsync();

                res.Data = item;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Đã có lỗi";
                res.ErrorCode = 500;
            }
            return res;
        }

        [HttpPost]
        [Route("api/slk/add")]
        public async Task<Reponse> AddNew(SLKAddRequest req)
        {
            Reponse res = new Reponse();
            string caLamGioBatDau = "";
            string caLamGioKetThuc = "";

            //validate
            if (req.caLam <= 0 || req.caLam >= 4)
            {
                res.Success = false;
                res.Message = "Ca làm không hợp lệ";
                res.ErrorCode = 400;
                return res;
            }
            if (string.IsNullOrEmpty(req.maCongViec.ToString()))
            {
                res.Success = false;
                res.Message = "Công việc không thể trống";
                res.ErrorCode = 400;
                return res;
            }
            if (string.IsNullOrEmpty(req.maSanPham.ToString()))
            {
                res.Success = false;
                res.Message = "Sản phẩm không thể trống";
                res.ErrorCode = 400;
                return res;
            }
            if (string.IsNullOrEmpty(req.maNhanCong.ToString()))
            {
                res.Success = false;
                res.Message = "Nhân công không thể trống";
                res.ErrorCode = 400;
                return res;
            }
            switch (req.caLam)
            {
                case 1:
                    caLamGioBatDau = "6:00:00";
                    caLamGioKetThuc = "14:00:00"; ;
                    break;
                case 2:
                    caLamGioBatDau = "14:00:00";
                    caLamGioKetThuc = "22:00:00";
                    break;
                case 3:
                    caLamGioBatDau = "22:00:00";
                    caLamGioKetThuc = "6:00:00";
                    break;
            }

            try
            {
                //add new day to nkslk
                var today = DateTime.Today;
                var item = await context.NKSLKs
                    .Where(t => DbFunctions.TruncateTime(t.ngay) >= today)
                    .FirstOrDefaultAsync();

                if (item == null)
                {
                    item = context.NKSLKs.Add(new NKSLK
                    {
                        ngay = DateTime.Now,
                    });
                }

                TimeSpan timeBatDau = TimeSpan.Parse(caLamGioBatDau);
                TimeSpan timeKetThuc = TimeSpan.Parse(caLamGioKetThuc);

                var nklsk_ct = new NKSLK_ChiTiet
                {
                    maNKSLK = item.maNKSLK,
                    maNhanCong = req.maNhanCong,
                    gioBatDau = timeBatDau,
                    gioKetThuc = timeKetThuc,
                };
                context.NKSLK_ChiTiet.Add(nklsk_ct);

                var danhmuckhoan_ct = new DanhMucKhoan_ChiTiet
                {
                    maNKSLK = item.maNKSLK,
                    maCongViec = req.maCongViec,
                    maSanPham = req.maSanPham,
                    soLoSanPham = req.soLoSanPham,
                    sanLuongThucTe = req.sanLuongThucTe,
                };
                context.DanhMucKhoan_ChiTiet.Add(danhmuckhoan_ct);

                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("nklsk", item);
                result.Add("nklsk_ct", nklsk_ct);
                result.Add("danhmuckhoan_ct", danhmuckhoan_ct);

                res.Success = true;
                res.Data = result;
                res.Message = "";

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                res.ErrorCode = 500;
                res.Success = false;
                res.Message = "Đã có lỗi";
            }
            return res;
        }

    }
}