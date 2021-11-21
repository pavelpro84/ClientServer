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

            var list = await context.Database.SqlQuery<SLK>(query, dateParams).ToListAsync();

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

            var list = await context.Database.SqlQuery<SLK>(query, dateParams).ToListAsync();

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
            var list = await context.Database.SqlQuery<SLK>(query, dateParams).ToListAsync();

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

        [HttpPost]
        [Route("api/slk/add")]
        public async Task<TimeSpan> AddNew(int maNhanCong, int maCongViec, int caLam)
        {
            string caLamGioBatDau = "";
            string caLamGioKetThuc = "";
            switch (caLam)
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

            TimeSpan timeBatDau = TimeSpan.Parse(caLamGioBatDau);
            TimeSpan timeKetThuc = TimeSpan.Parse(caLamGioKetThuc);

            return timeBatDau;
        }

    }
}