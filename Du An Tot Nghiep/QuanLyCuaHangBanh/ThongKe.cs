using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL_CuaHangBanh;
using DAL_CuaHangBanh;
using DTO_CuaHangBanh;

namespace GUI_CuaHangBanh
{
    public partial class ThongKe : Form
    {
        private BUSHoaDon bus = new BUSHoaDon();
        int maHoaDonDuocChon = -1;

        public ThongKe()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void frmThongKe_Load(object sender, EventArgs e)
        {
            LoadcmbSanPham();
            LoadcmbNhanVien();
            LoadDanhSachHoaDon();
        }
        private void LoadcmbSanPham()
        {
            BUSSanPham busSP = new BUSSanPham();
            cmbSanPham.DataSource = busSP.LayDanhSach();
            cmbSanPham.DisplayMember = "TenSanPham";
            cmbSanPham.ValueMember = "MaSanPham";
            cmbSanPham.SelectedIndex = -1;
        }
        private void LoadcmbNhanVien()
        {
            BUSNhanVien busNV = new BUSNhanVien();
            cmbMaNhanVien.DataSource = busNV.LayDanhSach();
            cmbMaNhanVien.DisplayMember = "MaNhanVien";
            cmbMaNhanVien.ValueMember = "MaNhanVien";
            cmbMaNhanVien.SelectedIndex = -1;
        }
        private void LoadDanhSachHoaDon()
        {
            dgvDSHD.DataSource = BUSThongKe.LayTatCaHoaDon();
            dgvDSHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Dictionary<string, string> columnHeaders = new Dictionary<string, string>
              {
        { "MaHoaDon", "Mã hóa đơn" },
        { "DateCheck", "Ngày tạo" },
        { "DateOut", "Ngày thanh toán" },
        { "TenBan", "Tên bàn" },
        { "TenNhanVien", "Nhân viên" },
        { "TenKhach", "Khách hàng" },
        { "TongHoaDon", "Tổng tiền" }
             };

            foreach (var col in columnHeaders)
            {
                if (dgvDSHD.Columns.Contains("TongHoaDon"))
                {
                    dgvDSHD.Columns["TongHoaDon"].DefaultCellStyle.Format = "N0";
                    dgvDSHD.Columns["TongHoaDon"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }
        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void dgvDSHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvDSHD_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDSHD.Columns.Contains("MaHoaDon"))
            {
                DataGridViewRow selectedRow = dgvDSHD.Rows[e.RowIndex];
                object cellValue = selectedRow.Cells["MaHoaDon"].Value;

                if (cellValue != null && int.TryParse(cellValue.ToString(), out int maHD))
                {
                    DataTable dtCT = BUSThongKe.LayChiTietHoaDon(maHD);

                    dgvChiTietHoaDon.DataSource = dtCT;
                    dgvChiTietHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dtCT.Columns.Contains("DonGia"))
                    {
                        dgvChiTietHoaDon.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                        dgvChiTietHoaDon.Columns["DonGia"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    decimal tongTien = 0;
                    foreach (DataRow row in dtCT.Rows)
                    {
                        if (int.TryParse(row["SoLuong"].ToString(), out int sl) &&
                            decimal.TryParse(row["DonGia"].ToString(), out decimal dg))
                        {
                            tongTien += sl * dg;
                        }
                    }

                    txtTongTienn.Text = tongTien.ToString("N0");
                }
                else
                {
                    MessageBox.Show("Không thể xác định mã hóa đơn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }




        private void btnTimKiemHD_Click(object sender, EventArgs e)
        {
            try
            {
                string maHD = txtMaHD.Text.Trim();
                string sql = "SELECT * FROM HoaDon WHERE MaHoaDon = @0";
                List<object> args = new List<object> { maHD };

                using (SqlDataReader reader = DBUtil.Query(sql, args))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvDSHD.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm hóa đơn: " + ex.Message);
            }
        }

        private void btnTimKiemSanPHAM_Click(object sender, EventArgs e)
        {
            try
            {
                string tenSP = cmbSanPham.Text.Trim();
                string sql = @"
            SELECT hd.* 
            FROM HoaDon hd 
            JOIN CT_HoaDon ct ON hd.MaHoaDon = ct.MaHoaDon
            JOIN SanPham sp ON ct.MaSanPham = sp.MaSanPham
            WHERE sp.TenSanPham LIKE '%' + @0 + '%'";

                List<object> args = new List<object> { tenSP };
                using (SqlDataReader reader = DBUtil.Query(sql, args))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvDSHD.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm theo sản phẩm: " + ex.Message);
            }
        }
        private void btnTimKiemMaNV_Click(object sender, EventArgs e)
        {

            try
            {
                string maNV = cmbMaNhanVien.SelectedValue?.ToString(); // hoặc cmbMaNhanVien.Text.Trim() nếu không dùng ValueMember

                string sql = @"
            SELECT * 
            FROM HoaDon 
            WHERE MaNhanVien = @0";

                List<object> args = new List<object> { maNV };

                using (SqlDataReader reader = DBUtil.Query(sql, args))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvDSHD.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm theo mã nhân viên: " + ex.Message);
            }
        }
        private void btnTimKiemTheoNgay_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime ngay = dtpNgay.Value.Date;
                string sql = "SELECT * FROM HoaDon WHERE CONVERT(date, DateCheck) = @0";
                List<object> args = new List<object> { ngay };

                using (SqlDataReader reader = DBUtil.Query(sql, args))
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvDSHD.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm theo ngày: " + ex.Message);
            }
        }

        private void btnLamMoiThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset mã hóa đơn
                if (txtMaHD != null)
                    txtMaHD.Clear();

                // Reset combobox sản phẩm
                if (cmbSanPham != null && cmbSanPham.Items.Count > 0)
                    cmbSanPham.SelectedIndex = -1;

                // Reset combobox nhân viên
                if (cmbMaNhanVien != null && cmbMaNhanVien.Items.Count > 0)
                    cmbMaNhanVien.SelectedIndex = -1;

                // Reset ngày
                if (dtpNgay != null)
                    dtpNgay.Value = DateTime.Now.Date;

                // Load lại danh sách hóa đơn
                if (dgvDSHD != null)
                    dgvDSHD.DataSource = BUSHoaDon.LayTatCaHoaDon();

                // Xóa bảng chi tiết và tổng tiền
                if (dgvChiTietHoaDon != null)
                    dgvChiTietHoaDon.DataSource = null;

                if (txtTongTien != null)
                    txtTongTien.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi làm mới thống kê: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaHoaDon_Click(object sender, EventArgs e)
        {
            if (dgvDSHD.CurrentRow != null)
            {
                string maHD = dgvDSHD.CurrentRow.Cells["MaHoaDon"].Value.ToString();
                int maHoaDon;

                if (int.TryParse(maHD, out maHoaDon))
                {
                    var result = MessageBox.Show("Bạn có chắc muốn xóa hóa đơn này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Lấy mã khách hàng từ dòng được chọn
                        string maKH = dgvDSHD.CurrentRow.Cells["MaKhach"].Value?.ToString();

                        // Kiểm tra số lượng hóa đơn của khách hàng này
                        string sqlCount = "SELECT COUNT(*) FROM HoaDon WHERE MaKhach = @0";
                        List<object> argsCount = new List<object> { maKH };

                        int soHoaDon = 0;
                        using (SqlDataReader reader = DBUtil.Query(sqlCount, argsCount))
                        {
                            if (reader.Read())
                            {
                                soHoaDon = reader.GetInt32(0);
                            }
                        }

                        // Xóa hóa đơn
                        bool thanhCong = bus.XoaHoaDon(maHoaDon);

                        if (thanhCong)
                        {
                            // Nếu khách chỉ có 1 hóa đơn => xóa luôn khách
                            if (soHoaDon == 1 && !string.IsNullOrEmpty(maKH))
                            {
                                BUSKhachHang busKH = new BUSKhachHang();
                                bool xoaKhach = busKH.XoaKhachHang(maKH);
                                if (xoaKhach)
                                {
                                    MessageBox.Show("Đã xóa khách hàng liên quan.");
                                }
                                else
                                {
                                    MessageBox.Show("Không thể xóa khách hàng.");
                                }
                            }

                            MessageBox.Show("Xóa hóa đơn thành công!");
                            dgvDSHD.DataSource = BUSHoaDon.LayTatCaHoaDon();
                        }
                        else
                        {
                            MessageBox.Show("Xóa hóa đơn thất bại!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mã hóa đơn không hợp lệ!");

                }
    }
}
    }
}
