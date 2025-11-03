using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Phong_Tro_GUI.ConTrolMain
{
    public partial class HoaDonMain : UserControl
    {
        private HoaDonBUS hoaDonBUS;
        private ChiTietHoaDonBUS chiTietBUS;
        private PhongBUS phongBUS;
        private string selectedMaHD;

        public HoaDonMain()
        {
            InitializeComponent();

            hoaDonBUS = new HoaDonBUS();
            chiTietBUS = new ChiTietHoaDonBUS();
            phongBUS = new PhongBUS();

            LoadHoaDon();
            LoadPhong();

            // Hook events
            dgvHoaDon.CellClick += DgvHoaDon_CellClick;
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
        }

        private void LoadHoaDon()
        {
            var list = hoaDonBUS.LayTatCa();
            dgvHoaDon.DataSource = list.Select(hd => new
            {
                hd.MaHD,
                TenKH = hd.HopDong?.KhachThue?.Ten,
                hd.GiaPhong,
                hd.TienDien,
                hd.TienNuoc,
                TienDichVu = chiTietBUS.LayTheoHoaDon(hd.MaHD).Sum(ct => (ct.DichVu?.DonGia ?? 0) * (ct.SoLuong ?? 0)),
                hd.TongTien,
                hd.NgayLap,
                hd.TrangThai
            }).ToList();
        }

        private void LoadPhong()
        {
            var list = phongBUS.LayTatCa();
            cbPhong.DataSource = list;
            cbPhong.DisplayMember = "TenPhong";
            cbPhong.ValueMember = "MaPhong";
        }

        private void DgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            selectedMaHD = dgvHoaDon.Rows[e.RowIndex].Cells["MaHD"].Value?.ToString();
            if (string.IsNullOrWhiteSpace(selectedMaHD)) return;

            var hd = hoaDonBUS.LayTheoMa(selectedMaHD);
            if (hd == null) return;

            txtMaHD.Text = hd.MaHD;
            txtTenKH.Text = hd.HopDong?.KhachThue?.Ten ?? "";
            dtpNgayLap.Value = hd.NgayLap ?? DateTime.Now;

            txtTienPhong.Text = (hd.GiaPhong ?? 0).ToString("N0");
            txtTienDien.Text = (hd.TienDien ?? 0).ToString("N0");
            txtTienNuoc.Text = (hd.TienNuoc ?? 0).ToString("N0");

            var chiTiet = chiTietBUS.LayTheoHoaDon(hd.MaHD);
            decimal tienDV = chiTiet.Sum(ct => (ct.DichVu?.DonGia ?? 0) * (ct.SoLuong ?? 0));
            txtTienDichVu.Text = tienDV.ToString("N0");

            txtTongTien.Text = hoaDonBUS.TinhTongTien(hd).ToString("N0");
            cbPhong.SelectedValue = hd.MaHopDong;
            cbTrangThai.SelectedItem = hd.TrangThai ?? "N/A";
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var hd = new HoaDon
                {
                    MaHD = txtMaHD.Text.Trim(),
                    MaHopDong = Convert.ToInt32(cbPhong.SelectedValue),
                    NgayLap = dtpNgayLap.Value,
                    GiaPhong = decimal.TryParse(txtTienPhong.Text, out var gp) ? gp : 0,
                    TienDien = decimal.TryParse(txtTienDien.Text, out var td) ? td : 0,
                    TienNuoc = decimal.TryParse(txtTienNuoc.Text, out var tn) ? tn : 0,
                    TienDichVu = 0,
                    TrangThai = cbTrangThai.SelectedItem?.ToString()
                };
                hoaDonBUS.Them(hd);
                LoadHoaDon();
                MessageBox.Show("✅ Thêm hóa đơn thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi thêm: " + ex.Message);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selectedMaHD)) return;
                var hd = hoaDonBUS.LayTheoMa(selectedMaHD);
                if (hd == null) return;

                hd.MaHopDong = Convert.ToInt32(cbPhong.SelectedValue);
                hd.GiaPhong = decimal.TryParse(txtTienPhong.Text, out var gp) ? gp : 0;
                hd.TienDien = decimal.TryParse(txtTienDien.Text, out var td) ? td : 0;
                hd.TienNuoc = decimal.TryParse(txtTienNuoc.Text, out var tn) ? tn : 0;
                hd.TienDichVu = chiTietBUS.LayTheoHoaDon(hd.MaHD).Sum(ct => (ct.DichVu?.DonGia ?? 0) * (ct.SoLuong ?? 0));
                hd.TrangThai = cbTrangThai.SelectedItem?.ToString();
                hd.NgayLap = dtpNgayLap.Value;

                hoaDonBUS.Sua(hd);
                LoadHoaDon();
                MessageBox.Show("✅ Cập nhật thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi sửa: " + ex.Message);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(selectedMaHD)) return;
                hoaDonBUS.Xoa(selectedMaHD);
                LoadHoaDon();
                MessageBox.Show("✅ Xóa hóa đơn thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xóa: " + ex.Message);
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaHD.Clear();
            txtTenKH.Clear();
            txtTienPhong.Clear();
            txtTienDien.Clear();
            txtTienNuoc.Clear();
            txtTienDichVu.Clear();
            txtTongTien.Clear();
            cbPhong.SelectedIndex = -1;
            cbTrangThai.SelectedIndex = -1;
            dtpNgayLap.Value = DateTime.Now;
            LoadHoaDon();
        }

        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            var keyword = txtTimKiem.Text.Trim();
            dgvHoaDon.DataSource = hoaDonBUS.TimKiem(keyword)
                .Select(hd => new
                {
                    hd.MaHD,
                    TenKH = hd.HopDong?.KhachThue?.Ten,
                    hd.GiaPhong,
                    hd.TienDien,
                    hd.TienNuoc,
                    TienDichVu = chiTietBUS.LayTheoHoaDon(hd.MaHD).Sum(ct => (ct.DichVu?.DonGia ?? 0) * (ct.SoLuong ?? 0)),
                    hd.TongTien,
                    hd.NgayLap,
                    hd.TrangThai
                }).ToList();
        }
    }
}
