using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DAL_CuaHangBanh
{
    public class DALThongKe
    {
        public static DataTable TK_HoaDon()
        {
            string sql = "sp_GetAllInvoices";
            SqlDataReader reader = DBUtil.Query(sql, null, CommandType.StoredProcedure);
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }

        public static DataTable TK_ChiTietHoaDon(string maHD)
        {
            string sql = "sp_GetInvoiceDetails";
            var parameters = new Dictionary<string, object>
             {
        { "@MaHoaDon", maHD }
            };

            SqlDataReader reader = DBUtil.QueryWithNamedParams(sql, parameters, CommandType.StoredProcedure);
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }
}
}
