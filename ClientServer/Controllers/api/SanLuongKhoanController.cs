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
            if (list == null)
            {
                return new NKSLK[] { };
            }
            return list;
        }

        [HttpGet]
        [Route("api/slk/thang")]
        public async Task<IEnumerable<SLK>> GetSLKThang(string date)
        {
            SqlParameter dateParams = new SqlParameter("@ThangLamViec", date);
            string query = "EXEC NKSLK_NhaMay @ThangLamViec";

            var list = await context.Database.SqlQuery<SLK>(query, dateParams).ToListAsync();
            return list;
        }

        [HttpGet]
        [Route("api/slk/tuan")]
        public async Task<IEnumerable<SLK>> GetSLKTuan(string date)
        {
            SqlParameter dateParams = new SqlParameter("@Date", date);
            string query = "EXEC NKSLK_NhaMay_Tuan @Date";

            var list = await context.Database.SqlQuery<SLK>(query, dateParams).ToListAsync();
            return list;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}