using ClientServer.Models;
using ClientServer.Models.QModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClientServer.Controllers.api
{
    public class LuongController : ApiController
    {
        private NKSLKDbContext context = new NKSLKDbContext();
        [HttpGet]
        [Route("api/luong/ngaycong")]
        public async Task<IEnumerable<NgayCong>> GetSLKThang(string date)
        {
            SqlParameter dateParams = new SqlParameter("@Date", date);
            string query = "EXEC Count_Ngay_Cong @Date";

            var list = await context.Database.SqlQuery<NgayCong>(query, dateParams).ToListAsync();
            return list;
        }
    }
}
