using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ClientServer.Models;
using ClientServer.Models.QModel;
using Newtonsoft.Json;

namespace ClientServer.Controllers.api
{
    public class SanPhamController : ApiController
    {
        private NKSLKDbContext context = new NKSLKDbContext();

        [HttpGet]
        [Route("api/sanpham")]
        public async Task<PagingData> GetAll(
            int? pageIndex = 0,
            int? pageSize = 10,
            string search = "",
            string sort = "id_asc")
        {
            var searchStr = SlugGenerator.SlugGenerator.GenerateSlug(search);
            var list = await context.SanPhams.AsNoTracking().ToListAsync();
            if (!String.IsNullOrEmpty(searchStr))
            {
                list = list.Where(x =>
                SlugGenerator.SlugGenerator
                    .GenerateSlug(x.tenSanPham)
                    .Contains(searchStr))
                    .ToList();
            }
            switch (sort)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.tenSanPham).ToList();
                    break;
                case "name_asc":
                    list = list.OrderBy(s => s.tenSanPham).ToList();
                    break;
                case "id_desc":
                    list = list.OrderByDescending(s => s.maSanPham).ToList();
                    break;
                case "id_asc":
                    list = list.OrderBy(s => s.maSanPham).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.maSanPham).ToList();
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
        [Route("api/sanpham/{id}")]
        public async Task<SanPham> GetById(int id)
        {
            return await context.SanPhams.FindAsync(id);
        }

        [HttpGet]
        [Route("api/sanpham/ndk")]
        public async Task<IEnumerable<SanPham>> GetProductNDK()
        {
            string query = "SELECT * FROM SanPham WHERE DATEDIFF(DAY, ngayDangKy, '08-15-2019') > 0";

            var list = await context.Database.SqlQuery<SanPham>(query).ToListAsync();
            return list;
        }

        [HttpGet]
        [Route("api/sanpham/hsd")]
        public async Task<IEnumerable<SanPham>> GetProductHSD()
        {
            string query = "SELECT *, DATEDIFF(DAY, SanPham.ngaySanXuat, SanPham.hanSuDung) AS NgayConLai FROM SanPham WHERE DATEDIFF(DAY, SanPham.ngaySanXuat, SanPham.hanSuDung) > 365";

            var list = await context.Database.SqlQuery<SanPham>(query).ToListAsync();
            return list;
        }

        [HttpPost]
        [Route("api/sanpham/add")]
        public async Task<SanPham> AddCongViec(SanPham sanpham)
        {
            context.SanPhams.Add(sanpham);
            await context.SaveChangesAsync();
            return sanpham;
        }

        [HttpPost]
        [Route("api/sanpham/edit/{id}")]
        public async Task<Boolean> EditSanPham(SanPham sanpham, int id)
        {
            SanPham sanpham_01 = context.SanPhams.Find(id);
            if (sanpham_01 == null)
            {
                return false;
            }
            sanpham_01.tenSanPham = sanpham.tenSanPham;
            sanpham_01.soDangKy = sanpham.soDangKy;
            sanpham_01.ngayDangKy = sanpham.ngayDangKy;
            sanpham_01.ngaySanXuat = sanpham.ngaySanXuat;
            sanpham_01.hanSuDung = sanpham.hanSuDung;
            sanpham_01.quyCach = sanpham.quyCach;
            await context.SaveChangesAsync();
            return true;

        }

        [HttpDelete]
        [Route("api/sanpham/delete/{id}")]
        public async Task<Boolean> DeleteSanPham(int id)
        {
            SanPham sanpham = context.SanPhams.Find(id);
            if (sanpham == null)
            {
                return false;
            }
            context.SanPhams.Remove(sanpham);
            await context.SaveChangesAsync();
            return true;

        }
    }
}