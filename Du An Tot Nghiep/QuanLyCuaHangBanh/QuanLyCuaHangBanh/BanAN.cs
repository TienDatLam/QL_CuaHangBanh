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
    public partial class BanAN : Form
    {
        BUSBanAn bus = new BUSBanAn();
        int selectedMaBan = -1;
        public BanAN()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += BanAn_Load;    
        }
        private void ClearForm()
        {
            txtMaBan.Text = "";
            txtTenBan.Text = "";
            rdbTrong.Checked = true;
            selectedMaBan = -1;
        }

        private void LoadDanhSachBanAn()
        {
            dgvBanAn.DataSource = bus.LayDanhSach();
            dgvBanAn.Columns["MaBan"].HeaderText = "Mã Bàn";
            dgvBanAn.Columns["TenBan"].HeaderText = "Tên Bàn";
            dgvBanAn.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }
        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void BanAn_Load(object sender, EventArgs e)
        {
            LoadDanhSachBanAn();   
            ClearForm();
            LayTrangThai();
        }
        private string LayTrangThai()
        {
            if (rdbTrong.Checked)
                return "Trống";
            else
                return "Đã dùng";
        }

        private void ChonTrangThai(string trangThai)
        {
            if (trangThai == "Trống")
                rdbTrong.Checked = true;
            else
                rdbCoKhach.Checked = true;
        }


        private void dgvBanAn_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBanAn.Rows[e.RowIndex];
                selectedMaBan = Convert.ToInt32(row.Cells["MaBan"].Value);
                txtMaBan.Text = selectedMaBan.ToString();
                txtTenBan.Text = row.Cells["TenBan"].Value.ToString();
                string trangThai = row.Cells["TrangThai"].Value.ToString();
                ChonTrangThai(trangThai);
            }
        }
    }
}
