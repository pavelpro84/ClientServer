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
        [Route("/api/sanpham/all")]
        public async Task<IEnumerable<SanPham>> Get()
        {
            List<SanPham> list = await context.SanPhams.AsNoTracking().ToListAsync();
            return list;
        }

        // GET api/<controller>/5
        public async Task<SanPham> Get(int id)
        {
            return await context.SanPhams.FindAsync(id);
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