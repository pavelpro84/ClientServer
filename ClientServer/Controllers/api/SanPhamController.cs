using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ClientServer.Models;
using Newtonsoft.Json;

namespace ClientServer.Controllers.api
{
    public class SanPhamController : ApiController
    {
        private NKSLKDbContext context = new NKSLKDbContext();

        [HttpGet]
        [Route("api/sanpham/all")]
        public async Task<IEnumerable<SanPham>> Get()
        {
            List<SanPham> list = await context.SanPhams.AsNoTracking().ToListAsync();
            return list;
        }
    }
}