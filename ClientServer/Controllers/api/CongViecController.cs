using ClientServer.Models;
using ClientServer.Models.QModel;
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
        public async Task<PagingData> GetAll(
            int? pageIndex = 0,
            int? pageSize = 10,
            string search = "",
            string sort = "id_asc")
        {
            var searchStr = SlugGenerator.SlugGenerator.GenerateSlug(search);
            var list = await context.CongViecs.AsNoTracking().ToListAsync();
            if (!String.IsNullOrEmpty(searchStr))
            {
                list = list.Where(x =>
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.tenCongViec)
                    .Contains(searchStr))
                    .ToList();
            }
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.tenCongViec).ToList();
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.tenCongViec).ToList();
                    break;
                case "price_desc":
                    list = list.OrderByDescending(s => s.donGia).ToList();
                    break;
                case "price_asc":
                    list = list.OrderBy(s => s.donGia).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maCongViec).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maCongViec).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maCongViec).ToList();
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
            congviec.donGia = 126360 * congviec.heSoKhoan * congviec.dinhMucLaoDong / congviec.dinhMucKhoan;
            context.CongViecs.Add(congviec);
            await context.SaveChangesAsync();
            return congviec;
        }

        //sua 1 phan tu 
        [HttpPost]
        [Route("api/congviec/edit/{id}")]
        public async Task<Boolean> EditCongViec(CongViec congviec, int id)
        {
            CongViec congviec_edit = context.CongViecs.Find(id);
            if (congviec_edit == null)
            {
                return false;
            }
            congviec_edit.tenCongViec = congviec.tenCongViec;
            congviec_edit.heSoKhoan = congviec.heSoKhoan;
            congviec_edit.donViKhoan = congviec.donViKhoan;
            congviec_edit.dinhMucLaoDong = congviec.dinhMucLaoDong;
            congviec_edit.dinhMucKhoan = congviec.dinhMucKhoan;
            congviec_edit.donGia = 126360 * congviec.heSoKhoan * congviec.dinhMucLaoDong / congviec.dinhMucKhoan;
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
