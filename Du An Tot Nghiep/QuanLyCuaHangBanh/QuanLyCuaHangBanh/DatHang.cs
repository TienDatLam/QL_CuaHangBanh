using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO_CuaHangBanh;
using BLL_CuaHangBanh;
using Guna.UI2.WinForms;

namespace GUI_CuaHangBanh
{
    public partial class DatHang : Form
    {
        public DatHang()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void DatHang_Load(object sender, EventArgs e)
        {
            LoadDSSanPham();
            pbSanPham.SizeMode = PictureBoxSizeMode.StretchImage;
            nudTongTien.Maximum = 100000000;

        }
        private List<DTOChiTietSPTheoBan> dsSanPhamTheoBan = new List<DTOChiTietSPTheoBan>();
        private void LoadDSSanPham()
        {
            List<DTOSanPham> ds = BUSSanPham.LayDanhSach();
            dgvSanPham.DataSource = ds;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSanPham.Columns["MaSanPham"].Visible = false;
            dgvSanPham.Columns["TenSanPham"].HeaderText = "Tên sản phẩm";
            dgvSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvSanPham.Columns["MoTa"].HeaderText = "Mô tả";
        }
        private void groupBox4_Enter(object sender, EventArgs e)
        {
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvSanPham.Rows[e.RowIndex];
                txtTenSP.Text = row.Cells["TenSanPham"].Value.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value.ToString();
            }
        }
        private string ChuyenTenSPThanhFileAnh(string tenSP)
        {
            // Ví dụ đơn giản: bỏ khoảng trắng, không dấu, thêm ".png"
            string ten = tenSP.ToLower().Replace(" ", "").Replace("đ", "d"); // thêm xử lý không dấu nếu muốn
            return ten + ".png";  // VD: "banhpucxi.png"
        }
        private void dgvSanPham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSanPham.Rows[e.RowIndex];

            // Lấy tên sản phẩm, đơn giá đổ vào textbox
            txtTenSP.Text = row.Cells["TenSanPham"].Value?.ToString();
            txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();

            // Lấy tên ảnh từ tên sản phẩm
            string tenSP = row.Cells["TenSanPham"].Value?.ToString();
            if (!string.IsNullOrWhiteSpace(tenSP))
            {
                string tenFileAnh = ChuyenTenSPThanhFileAnh(tenSP);
                string path = Path.Combine(Application.StartupPath, tenFileAnh);

                if (File.Exists(path))
                    pbSanPham.Image = Image.FromFile(path);
                else
                    pbSanPham.Image = null;
            }
        }
        private void btnThemSP_Click(object sender, EventArgs e)
        {
            if (dgvSanPham.CurrentRow != null)
            {
                string tenSP = dgvSanPham.CurrentRow.Cells["TenSanPham"].Value.ToString();
                int donGia = Convert.ToInt32(dgvSanPham.CurrentRow.Cells["DonGia"].Value);
                int soLuong = (int)nudSoLuong.Value;
                var spDaCo = dsSanPhamTheoBan.FirstOrDefault(sp => sp.TenSanPham == tenSP);
                if (spDaCo != null)
                {
                    spDaCo.SoLuong += soLuong;
                }
                if (soLuong <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số lượng sản phẩm hợp lệ!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    DTOChiTietSPTheoBan spMoi = new DTOChiTietSPTheoBan
                    {
                        TenSanPham = tenSP,
                        DonGia = donGia,
                        SoLuong = soLuong
                    };
                    dsSanPhamTheoBan.Add(spMoi);
                }

                CapNhatGridViewSanPhamTheoBan();
                CapNhatTongTien();
            }
        }
        private void CapNhatGridViewSanPhamTheoBan()
        {
            dgvSanPhamTheoBan.DataSource = null;
            dgvSanPhamTheoBan.DataSource = dsSanPhamTheoBan;
            dgvSanPhamTheoBan.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dgvSanPhamTheoBan.Columns["SoLuong"].HeaderText = "Số Lượng";
            dgvSanPhamTheoBan.Columns["DonGia"].HeaderText = "Đơn Giá";
            dgvSanPhamTheoBan.Columns["ThanhTien"].HeaderText = "Tổng Đơn Giá";
            dgvSanPhamTheoBan.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvSanPhamTheoBan.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }
        private void CapNhatTongTien()
        {
            int tong = dsSanPhamTheoBan.Sum(sp => sp.ThanhTien);
            decimal tongTien = 0;

            foreach (DataGridViewRow row in dgvSanPhamTheoBan.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    tongTien += Convert.ToDecimal(row.Cells["ThanhTien"].Value);
                }
            }

            // Cập nhật Maximum nếu cần
            if (tongTien > nudTongTien.Maximum)
            {
                nudTongTien.Maximum = tongTien + 100000; // thêm biên để tránh lỗi
            }

            nudTongTien.Value = tongTien;   
        }
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
        }
        private int indexSelected = -1;
        private void btnSuaSP_Click(object sender, EventArgs e)
        {
            if (indexSelected >= 0 && indexSelected < dsSanPhamTheoBan.Count)
            {
                dsSanPhamTheoBan[indexSelected].SoLuong = (int)nudSoLuong.Value;

                CapNhatGridViewSanPhamTheoBan();
                CapNhatTongTien();

                // Reset
                indexSelected = -1;
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
            }
        }
        private void dgvSanPhamTheoBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dsSanPhamTheoBan.Count)
            {
                indexSelected = e.RowIndex;

                var selected = dsSanPhamTheoBan[indexSelected];
                txtTenSP.Text = selected.TenSanPham;
                txtDonGia.Text = selected.DonGia.ToString("N0");
                nudSoLuong.Value = selected.SoLuong;
                // txtDonGia.Text = selected.DonGia.ToString("N0"); // nếu có

                // Có thể bật nút sửa/xóa tại đây
                btnSuaSP.Enabled = true;
                btnXoaSP.Enabled = true;
            }
        }
        private void btnXoaSP_Click(object sender, EventArgs e)
        {
            if (indexSelected >= 0 && indexSelected < dsSanPhamTheoBan.Count)
            {
                dsSanPhamTheoBan.RemoveAt(indexSelected);

                CapNhatGridViewSanPhamTheoBan();
                CapNhatTongTien();

                indexSelected = -1;
                btnSuaSP.Enabled = false;
                btnXoaSP.Enabled = false;
            }
        }
        private void btnThemTTKH_Click(object sender, EventArgs e)
        {
               
            }
    }
}



