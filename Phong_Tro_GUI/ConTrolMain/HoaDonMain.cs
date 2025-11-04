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
        private Connect connect;

        public HoaDonMain()
        {
            InitializeComponent();
            hoaDonBUS = new HoaDonBUS();
            connect = new Connect();

            LoadData();
            LoadComboBoxPhong();
            HookEvents();
        }

        private void HookEvents()
        {
            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
            btnTimKiem.Click += BtnTimKiem_Click;
            btnGuiThongBao.Click += BtnGuiThongBao_Click;
            dgvHoaDon.SelectionChanged += DgvHoaDon_SelectionChanged;
        }

        private void LoadData()
        {
            dgvHoaDon.DataSource = hoaDonBUS.GetAll()
                .Select(h => new
                {
                    h.MaHD,
                    TenKH = h.HopDong.KhachThue.Ten,
                    Phong = h.HopDong.Phong.TenPhong,
                    h.NgayLap,
                    h.TienDien,
                    h.TienNuoc,
                    h.TienDichVu,
                    h.GiaPhong,
                    h.TongTien,
                    h.TrangThai
                }).ToList();
            ClearForm();
        }

        private void LoadComboBoxPhong()
        {
            var phongs = connect.Phongs.ToList();

            cbPhong.DataSource = phongs;
            cbPhong.DisplayMember = "TenPhong";
            cbPhong.ValueMember = "MaPhong";

            cbPhongThongBao.DataSource = phongs;
            cbPhongThongBao.DisplayMember = "TenPhong";
            cbPhongThongBao.ValueMember = "MaPhong";
        }

        private void ClearForm()
        {
            txtMaHD.Clear();
            txtTenKH.Clear();
            cbPhong.SelectedIndex = -1;
            dtpNgayLap.Value = DateTime.Now;
            txtTienPhong.Clear();
            txtTienDien.Clear();
            txtTienNuoc.Clear();
            txtTienDichVu.Clear();
            txtTongTien.Clear();
            cbTrangThai.SelectedIndex = -1;
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
                    GiaPhong = string.IsNullOrEmpty(txtTienPhong.Text) ? 0 : Convert.ToDecimal(txtTienPhong.Text),
                    TienDien = string.IsNullOrEmpty(txtTienDien.Text) ? 0 : Convert.ToDecimal(txtTienDien.Text),
                    TienNuoc = string.IsNullOrEmpty(txtTienNuoc.Text) ? 0 : Convert.ToDecimal(txtTienNuoc.Text),
                    TienDichVu = string.IsNullOrEmpty(txtTienDichVu.Text) ? 0 : Convert.ToDecimal(txtTienDichVu.Text),
                    TrangThai = cbTrangThai.SelectedItem?.ToString()
                };

                if (hoaDonBUS.Add(hd))
                {
                    MessageBox.Show("Thêm hóa đơn thành công!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Thêm hóa đơn thất bại!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            try
            {
                var hd = hoaDonBUS.GetById(txtMaHD.Text.Trim());
                if (hd == null)
                {
                    MessageBox.Show("Hóa đơn không tồn tại!");
                    return;
                }

                hd.GiaPhong = string.IsNullOrEmpty(txtTienPhong.Text) ? 0 : Convert.ToDecimal(txtTienPhong.Text);
                hd.TienDien = string.IsNullOrEmpty(txtTienDien.Text) ? 0 : Convert.ToDecimal(txtTienDien.Text);
                hd.TienNuoc = string.IsNullOrEmpty(txtTienNuoc.Text) ? 0 : Convert.ToDecimal(txtTienNuoc.Text);
                hd.TienDichVu = string.IsNullOrEmpty(txtTienDichVu.Text) ? 0 : Convert.ToDecimal(txtTienDichVu.Text);
                hd.TrangThai = cbTrangThai.SelectedItem?.ToString();

                if (hoaDonBUS.Update(hd))
                {
                    MessageBox.Show("Cập nhật hóa đơn thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Cập nhật hóa đơn thất bại!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            var maHD = txtMaHD.Text.Trim();
            if (string.IsNullOrEmpty(maHD))
            {
                MessageBox.Show("Chọn hóa đơn để xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa hóa đơn này?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (hoaDonBUS.Delete(maHD))
                {
                    MessageBox.Show("Xóa hóa đơn thành công!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Xóa hóa đơn thất bại!");
                }
            }
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            var keyword = txtTimKiem.Text.Trim().ToLower();
            dgvHoaDon.DataSource = hoaDonBUS.GetAll()
                .Where(h => h.MaHD.ToLower().Contains(keyword) ||
                            h.HopDong.KhachThue.Ten.ToLower().Contains(keyword))
                .Select(h => new
                {
                    h.MaHD,
                    TenKH = h.HopDong.KhachThue.Ten,
                    Phong = h.HopDong.Phong.TenPhong,
                    h.NgayLap,
                    h.TienDien,
                    h.TienNuoc,
                    h.TienDichVu,
                    h.GiaPhong,
                    h.TongTien,
                    h.TrangThai
                }).ToList();
        }

        private void DgvHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count > 0)
            {
                var row = dgvHoaDon.SelectedRows[0];
                txtMaHD.Text = row.Cells["MaHD"].Value.ToString();
                txtTenKH.Text = row.Cells["TenKH"].Value.ToString();
                cbPhong.Text = row.Cells["Phong"].Value.ToString();
                dtpNgayLap.Value = Convert.ToDateTime(row.Cells["NgayLap"].Value);
                txtTienPhong.Text = row.Cells["GiaPhong"].Value.ToString();
                txtTienDien.Text = row.Cells["TienDien"].Value.ToString();
                txtTienNuoc.Text = row.Cells["TienNuoc"].Value.ToString();
                txtTienDichVu.Text = row.Cells["TienDichVu"].Value.ToString();
                txtTongTien.Text = row.Cells["TongTien"].Value?.ToString() ?? "0";
                cbTrangThai.Text = row.Cells["TrangThai"].Value?.ToString() ?? "";

            }
        }

        // 🔹 Phần gửi thông báo theo Cách 2
        private void BtnGuiThongBao_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy HopDong theo phòng được chọn
                var hopDong = connect.HopDongs
                    .FirstOrDefault(h => h.MaPhong == cbPhongThongBao.SelectedValue.ToString());

                if (hopDong == null)
                {
                    MessageBox.Show("Phòng chưa có khách thuê!");
                    return;
                }

                // Gửi thông báo tới khách thuê
                var thongBao = new ThongBao
                {
                    MaTK_Gui = 1, // admin
                    MaTK_Nhan = hopDong.KhachThue.MaKhach,
                    NoiDung = txtNoiDungThongBao.Text,
                    NgayGui = DateTime.Now
                };

                connect.ThongBaos.Add(thongBao);
                connect.SaveChanges();

                MessageBox.Show("Gửi thông báo thành công!");
                txtNoiDungThongBao.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi thông báo: " + ex.Message);
            }
        }
    }
}
