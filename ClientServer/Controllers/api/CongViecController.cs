using ClientServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ClientServer.Controllers.api
{
    public class CongViecController : ApiController
    {
        private NKSLKDbContext context = new NKSLKDbContext();

        [HttpGet]
        [Route("api/congviec")]
        public async Task<IEnumerable<CongViec>> GetAll(int? pageIndex = 0, int? pageSize = 10)
        {
            return await context.CongViecs
                .OrderBy(x => x.maCongViec)
                .Skip(pageIndex.Value * pageSize.Value)  
                .Take(pageSize.Value)
                .ToListAsync();
        }
        [HttpGet]
        [Route("api/congviec/{id}")]
        public async Task<CongViec> GetById(int id)
        {
            return await context.CongViecs.FindAsync(id);
        }

        // them 1 phan tu
        [HttpPost]
        [Route("api/congviec/add")]
        public async Task<CongViec> AddCongViec(CongViec congviec)
        {
            context.CongViecs.Add(congviec);
            await context.SaveChangesAsync();
            return congviec;
        }

        //sua 1 phan tu 
        [HttpPost]
        [Route("api/congviec/edit/{id}")]
        public async Task<Boolean> EditCongViec(CongViec congviec, int id)
        {
            CongViec congviec_01 = context.CongViecs.Find(id);
            if (congviec_01 == null)
            {
                return false;
            }
            congviec_01.tenCongViec = congviec.tenCongViec;
            congviec_01.heSoKhoan = congviec.heSoKhoan;
            congviec_01.donViKhoan = congviec.donViKhoan;
            congviec_01.donGia = congviec.heSoKhoan *126360;
            congviec_01.dinhMucLaoDong = congviec.dinhMucLaoDong;
            congviec_01.dinhMucKhoan = congviec.dinhMucKhoan;
            await context.SaveChangesAsync();
            return true;

        }


        // xoa 1 phan tu
        [HttpDelete]
        [Route("api/congviec/delete/{id}")]
        public async  Task<Boolean> DeleteCongViec(int id)
        {
            CongViec congviec = context.CongViecs.Find(id);
            if (congviec == null)
            {
                return false;
            }
            context.CongViecs.Remove(congviec);
            await context.SaveChangesAsync();
            return true ;

        }
    }
}
