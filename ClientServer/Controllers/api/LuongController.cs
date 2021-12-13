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

        [HttpGet]
        [Route("api/luong/sanpham")]
        public async Task<IEnumerable<LuongRes>> GetLuongSP(string date, string type)
        {
            string query = $"DECLARE @DATE DATETIME='{date}';"
                +
                @"WITH NKSLK_KhoanChung(maCongViec, maNKSLK, SoLuong) AS (
	                SELECT CongViec.maCongViec, NKSLK.maNKSLK, Count(NhanCong.maNhanCong) AS SoLuong
	                FROM NKSLK, NKSLK_ChiTiet, NhanCong, CongViec
	                WHERE
	                NKSLK.maNKSLK = NKSLK_ChiTiet.maNKSLK
	                AND NKSLK_ChiTiet.maNhanCong = NhanCong.maNhanCong
	                AND CongViec.maCongViec = NKSLK_ChiTiet.maCongViec
	                AND NKSLK.ngay BETWEEN @DATE-DAY(@DATE)+1 and EOMONTH(@DATE)
	                GROUP BY CongViec.maCongViec, NKSLK.maNKSLK
                )
                SELECT NhanCong.maNhanCong, NhanCong.hoTen,
                (
	                CASE WHEN (NKSLK_KhoanChung.SoLuong = 1)
	                THEN SUM((NKSLK_ChiTiet.sanLuongThucTe * CongViec.donGia))
	                ELSE SUM((NKSLK_ChiTiet.sanLuongThucTe * CongViec.donGia) * ABS(DATEDIFF(HOUR, NKSLK_ChiTiet.gioBatDau, NKSLK_ChiTiet.gioKetThuc) / 8))
	                END
                ) AS Luong
                FROM NKSLK_KhoanChung, NKSLK_ChiTiet, NhanCong, CongViec
                WHERE 
                NKSLK_KhoanChung.maNKSLK = NKSLK_ChiTiet.maNKSLK
                AND NKSLK_ChiTiet.maNhanCong = NhanCong.maNhanCong
                AND CongViec.maCongViec = NKSLK_ChiTiet.maCongViec
                GROUP BY NhanCong.maNhanCong, NhanCong.hoTen, NKSLK_KhoanChung.SoLuong";

            var list = await context.Database.SqlQuery<LuongRes>(query).ToListAsync();

            if (type.Equals("max")) list = list.OrderByDescending(x => x.Luong).ToList();
            else if (type.Equals("min")) list = list.OrderBy(x => x.Luong).ToList();

            return list;
        }
    }
}
