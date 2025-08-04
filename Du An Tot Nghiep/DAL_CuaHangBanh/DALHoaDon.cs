using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DTO_CuaHangBanh;

namespace DAL_CuaHangBanh
{
    public class DALHoaDon
    {
        public void InsertHoaDon(DTOHoaDon hd)
        {
            string sql = "INSERT INTO HoaDon (DateCheck, DateOut, MaNhanVien, MaKhachHang, MaBan, TrangThai, TongHoaDon, GiamGia) " +
                         "VALUES (@0, @1, @2, @3, @4, @5, @6, @7)";

            List<object> args = new List<object>
            {
                hd.DateCheck,
                hd.DateOut,
                hd.MaNhanVien,
                hd.MaKhachHang,
                hd.MaBan,
                hd.TrangThai,
                hd.TongHoaDon,
                hd.GiamGia
            };

            DBUtil.Update(sql, args);
        }
        public bool XoaHoaDon(int maHoaDon)
        {
            try
            {
              
                string sqlCT = "DELETE FROM ChiTietHoaDon WHERE MaHoaDon = @0";
                DBUtil.Update(sqlCT, new List<object> { maHoaDon });

                // Sau đó xóa Hóa Đơn
                string sqlHD = "DELETE FROM HoaDon WHERE MaHoaDon = @0";
                DBUtil.Update(sqlHD, new List<object> { maHoaDon });

                return true;
            }
            catch
            {
                return false;
            }
        }


        public DTOHoaDon GetLastHoaDon()
        {
            string sql = @"
        SELECT TOP 1 
            hd.MaHoaDon,
            hd.DateCheck,
            hd.DateOut,
            hd.MaNhanVien,
            hd.MaKhachHang,
            hd.MaBan,
            b.TenBan,
            hd.TrangThai,
            hd.TongHoaDon,
            hd.GiamGia
        FROM HoaDon hd
        JOIN Ban b ON hd.MaBan = b.MaBan
        ORDER BY MaHoaDon DESC";

            DTOHoaDon hd = null;

            using (SqlDataReader reader = DBUtil.Query(sql, new List<object>()))
            {
                if (reader.Read())
                {
                    hd = new DTOHoaDon
                    {
                        MaHoaDon = reader["MaHoaDon"]?.ToString(),
                        DateCheck = reader["DateCheck"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["DateCheck"]),
                        DateOut = reader["DateOut"] is DBNull ? DateTime.Now : Convert.ToDateTime(reader["DateOut"]),
                        MaNhanVien = reader["MaNhanVien"]?.ToString(),
                        MaKhachHang = reader["MaKhachHang"]?.ToString(),
                        MaBan = Convert.ToInt32(reader["MaBan"]),
                        TenBan = reader["TenBan"]?.ToString(), 
                        TrangThai = reader["TrangThai"]?.ToString(),
                        TongHoaDon = reader["TongHoaDon"] is DBNull ? 0 : Convert.ToDecimal(reader["TongHoaDon"]),
                        GiamGia = reader["GiamGia"] is DBNull ? 0 : Convert.ToDecimal(reader["GiamGia"])
                    };
                }
            }

            return hd;
        }

    }
}