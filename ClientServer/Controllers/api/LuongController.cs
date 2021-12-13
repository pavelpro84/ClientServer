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
            string query = $"DECLARE @DATE DATETIME='{date}';"
                 +
                 @"SELECT SUM(
                    CASE WHEN (CONVERT(TIME, NKSLK_ChiTiet.gioBatDau) >= CONVERT(TIME, '22:00:00') 
                        AND CONVERT(TIME, NKSLK_ChiTiet.gioKetThuc) <= CONVERT(TIME, '6:00:00'))
                    THEN (24 - DATEPART(hour,NKSLK_ChiTiet.gioBatDau) + DATEPART(hour,NKSLK_ChiTiet.gioKetThuc)) / 8 * 1.3
                    ELSE ABS(DATEDIFF(HOUR, NKSLK_ChiTiet.gioBatDau, NKSLK_ChiTiet.gioKetThuc)) / 8
                    END
                )as SoCong, NhanCong.hoTen, NhanCong.maNhanCong
                FROM NhanCong, NKSLK, NKSLK_ChiTiet
                WHERE NKSLK_ChiTiet.maNhanCong = NhanCong.maNhanCong
                and NKSLK.maNKSLK = NKSLK_ChiTiet.maNKSLK
                AND NKSLK.ngay BETWEEN @DATE-DAY(@DATE)+1 and EOMONTH(@DATE)
                Group by NhanCong.hoTen, NhanCong.maNhanCong";

            var list = await context.Database.SqlQuery<NgayCong>(query).ToListAsync();

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
