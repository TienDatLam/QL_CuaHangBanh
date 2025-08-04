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
    public partial class SanPham : Form
    {
        private BUSSanPham busSP = new BUSSanPham();
        public SanPham()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

        }
        private void SanPham_Load(object sender, EventArgs e)
        {
            LoadSanPham();
        }
        private void LoadSanPham()
        {
            dgvSanPham.DataSource = busSP.LayDanhSach();
            dgvSanPham.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dgvSanPham.Columns["DonGia"].HeaderText = "Đơn Giá";
            dgvSanPham.Columns["TenDanhMuc"].HeaderText = "Tên Danh Mục";
            dgvSanPham.Columns["So Luong"].HeaderText = "Số Lượng";
            dgvSanPham.Columns["MoTa"].HeaderText = "Mô Tả";
        }
    }
}
