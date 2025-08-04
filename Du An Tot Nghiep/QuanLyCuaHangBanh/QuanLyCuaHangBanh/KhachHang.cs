using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL_CuaHangBanh;
using DTO_CuaHangBanh;

namespace GUI_CuaHangBanh
{
    public partial class KhachHang : Form
    {
        private BUSKhachHang bus = new BUSKhachHang();

        public KhachHang()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += KhachHang_Load;

        }
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void KhachHang_Load(object sender, EventArgs e)
        {
           LoadDanhSachKhacHhang();
        }
        private void LoadDanhSachKhacHhang()
        {
            List<DTOKhachHang> ds = bus.LayDanhSachKhachHang();
            dgvKhachHang.DataSource = ds;
            dgvKhachHang.Columns["MaKH"].HeaderText = "Mã KH";
            dgvKhachHang.Columns["TenKH"].HeaderText = "Tên khách hàng";
            dgvKhachHang.Columns["SDT"].HeaderText = "SĐT";
        }
    }
}
