using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_CuaHangBanh;

namespace BLL_CuaHangBanh
{
    public class BUSThongKe
    {
        DALThongKe dal = new DALThongKe();


        public static DataTable LayTatCaHoaDon()
        {
            return DBUtil.GetAllInvoices(); 
        }

        public static DataTable LayChiTietHoaDon(int maHD)
        {
            return DBUtil.GetInvoiceDetails(maHD); 
        }
}
}