    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using DTO_CuaHangBanh;

    namespace DAL_CuaHangBanh
    {
        public class DALCTHoaDon
        {
            public int InsertCTHoaDon(DTOCTHoaDon ct)
            {
                string sql = "INSERT INTO CT_HoaDon (MaHoaDon, MaSanPham, SoLuong) " +
                             "VALUES (@0, @1, @2)";

                List<object> parameters = new List<object>
                {
                    ct.MaHoaDon,
                    ct.MaSanPham,
                    ct.SoLuong
                };

                DBUtil.Update(sql, parameters); 
                return 1; 
            }

            public List<DTOCTHoaDon> GetCTHoaDonByMaHD(string maHD)
            {
                string sql = "SELECT * FROM CT_HoaDon WHERE MaHoaDon = @0";
                List<object> parameters = new List<object>
                {
                    maHD
                };

                List<DTOCTHoaDon> list = new List<DTOCTHoaDon>();
                var reader = DBUtil.Query(sql, parameters);

                while (reader.Read())
                {
                    DTOCTHoaDon ct = new DTOCTHoaDon
                    {
                        MaCTHoaDon = reader["MaCTHoaDon"].ToString(),
                        MaHoaDon = reader["MaHoaDon"].ToString(),
                        MaSanPham = reader["MaSanPham"].ToString(),
                        SoLuong = Convert.ToInt32(reader["SoLuong"])
                    };

                    list.Add(ct);
                }

                reader.Close();
                return list;
            }
     
        
        }

    }


