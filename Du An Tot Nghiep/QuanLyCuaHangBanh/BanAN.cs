using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL_CuaHangBanh;
using DAL_CuaHangBanh;
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
            dgvBanAn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBanAn.DataSource = bus.LayDanhSach();
            dgvBanAn.Columns["MaBan"].HeaderText = "Mã Bàn";
            dgvBanAn.Columns["TenBan"].HeaderText = "Tên Bàn";
            dgvBanAn.Columns["TrangThai"].HeaderText = "Trạng Thái";
        }

        private void BanAn_Load(object sender, EventArgs e)
        {
            LoadDanhSachBanAn();
            ClearForm();
        }

        private string LayTrangThai()
        {
            return rdbTrong.Checked ? "Trống" : "Đã dùng";
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
                var row = dgvBanAn.Rows[e.RowIndex];
                selectedMaBan = Convert.ToInt32(row.Cells["MaBan"].Value);
                txtMaBan.Text = selectedMaBan.ToString();
                txtTenBan.Text = row.Cells["TenBan"].Value.ToString();

                string trangThaiText = row.Cells["TrangThai"].Value.ToString();
                ChonTrangThai(trangThaiText);
            }
        }

        private void btnThemBan_Click(object sender, EventArgs e)
        {
            string tenBan = txtTenBan.Text.Trim();
            string trangThai = LayTrangThai();

            DTOBanAn ban = new DTOBanAn(0, tenBan, trangThai);
            bus.Them(ban);
            LoadDanhSachBanAn();
            ClearForm();
        }

        private void btnSuaBan_Click(object sender, EventArgs e)
        {
            if (selectedMaBan == -1)
            {
                MessageBox.Show("Vui lòng chọn bàn để sửa.");
                return;
            }

            string tenBan = txtTenBan.Text.Trim();
            string trangThai = LayTrangThai();

            DTOBanAn ban = new DTOBanAn(selectedMaBan, tenBan, trangThai);
            bus.CapNhat(ban);
            LoadDanhSachBanAn();
            ClearForm();
        }

        private void btnXoaBan_Click(object sender, EventArgs e)
        {
            if (selectedMaBan == -1)
            {
                MessageBox.Show("Vui lòng chọn bàn để xóa.");
                return;
            }

            bus.Xoa(selectedMaBan);
            LoadDanhSachBanAn();
            ClearForm();
        }

        private void btnLamMoiBan_Click(object sender, EventArgs e)
        {
            txtMaBan.Text = "";
            txtTenBan.Text = "";
            rdbTrong.Checked = true;
            txtMaBan.Focus();
            txtTimBan.Text = "";
        }
        private BUSBanAn busBA = new BUSBanAn();

        private void btnTimKiemBan_Click(object sender, EventArgs e)
        {
            string tuKhoa = txtTimBan.Text.ToLower(); 

            var ds = busBA.LayDanhSach()
                               .Where(ba => ba.TenBan.ToLower().Contains(tuKhoa))
                               .Select(ba => new
                               {
                                   ba.MaBan,
                                   ba.TenBan,
                               }).ToList();

            dgvBanAn.DataSource = ds;
        }



        private void txtTenBan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}