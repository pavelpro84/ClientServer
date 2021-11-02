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
        public async Task<IEnumerable<NhanCong>> GetAll(int? pageIndex = 0, int? pageSize = 10)
        {
            return await context.NhanCongs
                .OrderBy(x => x.maNhanCong)
                .Skip(pageIndex.Value * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
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
    }
}