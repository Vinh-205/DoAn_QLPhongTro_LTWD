using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_GUI.ConTrolMain
{
    public partial class ThongBaoMain : UserControl
    {
        private readonly ThongBaoBUS _thongBaoBUS = new ThongBaoBUS();
        private readonly PhongBUS _phongBUS = new PhongBUS();

        public ThongBaoMain()
        {
            InitializeComponent();
            Load += ThongBaoMain_Load;

            // Gắn sự kiện
            btnGui.Click += btnGui_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLamMoi.Click += btnLamMoi_Click;
            dgvThongBao.CellClick += dgvThongBao_CellClick;

            // Fake placeholder cho TextBox nếu dùng .NET Framework
            txtNoiDung.ForeColor = Color.Gray;
            txtNoiDung.Text = "Nhập nội dung thông báo...";
            txtNoiDung.Enter += (s, e) =>
            {
                if (txtNoiDung.Text == "Nhập nội dung thông báo...")
                {
                    txtNoiDung.Text = "";
                    txtNoiDung.ForeColor = Color.Black;
                }
            };
            txtNoiDung.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtNoiDung.Text))
                {
                    txtNoiDung.Text = "Nhập nội dung thông báo...";
                    txtNoiDung.ForeColor = Color.Gray;
                }
            };
        }

        private void ThongBaoMain_Load(object sender, EventArgs e)
        {
            TaiDanhSachNguoiNhan();
            TaiDanhSachThongBao();
            dtNgayGui.Value = DateTime.Now;
        }

        // ==================== LOAD NGƯỜI NHẬN ====================
        private void TaiDanhSachNguoiNhan()
        {
            var dsPhong = _phongBUS.LayTatCa();
            cboNguoiNhan.DataSource = dsPhong;
            cboNguoiNhan.DisplayMember = "TenPhong";
            cboNguoiNhan.ValueMember = "MaPhong";
            cboNguoiNhan.SelectedIndex = -1;
        }

        // ==================== LOAD DANH SÁCH THÔNG BÁO ====================
        private void TaiDanhSachThongBao()
        {
            var ds = _thongBaoBUS.LayTatCa();
            dgvThongBao.DataSource = ds.Select(tb => new
            {
                tb.MaTB,
                NguoiNhan = tb.MaTK_Nhan,
                NoiDung = tb.NoiDung,
                NgayGui = tb.NgayGui.HasValue ? tb.NgayGui.Value.ToString("dd/MM/yyyy HH:mm") : "",
                NgayTao = tb.NgayGui.HasValue ? tb.NgayGui.Value.ToString("dd/MM/yyyy HH:mm") : ""
            }).ToList();

            dgvThongBao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThongBao.ClearSelection();
        }


        // ==================== THÊM THÔNG BÁO ====================
        private void btnGui_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNoiDung.Text) || txtNoiDung.ForeColor == Color.Gray)
            {
                MessageBox.Show("Vui lòng nhập nội dung thông báo.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboNguoiNhan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn người nhận.", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var tb = new ThongBao
                {
                    MaTK_Gui = 1, // ví dụ: admin
                    MaTK_Nhan = Convert.ToInt32(cboNguoiNhan.SelectedValue),
                    NoiDung = txtNoiDung.Text.Trim(),
                    NgayGui = DateTime.Now,
                    DaDoc = false
                };

                if (_thongBaoBUS.Them(tb))
                {
                    MessageBox.Show("Gửi thông báo thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLamMoi.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi thông báo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== SỬA THÔNG BÁO ====================
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvThongBao.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thông báo để sửa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTB = Convert.ToInt32(dgvThongBao.CurrentRow.Cells["MaTB"].Value);

            try
            {
                var tb = new ThongBao
                {
                    MaTB = maTB,
                    NoiDung = txtNoiDung.Text.Trim(),
                    MaTK_Nhan = Convert.ToInt32(cboNguoiNhan.SelectedValue),
                    NgayGui = dtNgayGui.Value,
                    DaDoc = false
                };

                if (_thongBaoBUS.Sua(tb))
                {
                    MessageBox.Show("Đã cập nhật thông báo!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSachThongBao();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật thông báo: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== XÓA THÔNG BÁO ====================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvThongBao.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn thông báo cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maTB = Convert.ToInt32(dgvThongBao.CurrentRow.Cells["MaTB"].Value);

            if (MessageBox.Show("Bạn có chắc muốn xóa thông báo này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (_thongBaoBUS.Xoa(maTB))
                    {
                        MessageBox.Show("Đã xóa thông báo!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TaiDanhSachThongBao();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa thông báo: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ==================== LÀM MỚI ====================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtNoiDung.Clear();
            txtNoiDung.ForeColor = Color.Gray;
            txtNoiDung.Text = "Nhập nội dung thông báo...";
            cboNguoiNhan.SelectedIndex = -1;
            dtNgayGui.Value = DateTime.Now;
            TaiDanhSachThongBao();
        }

        // ==================== CLICK DỮ LIỆU ====================
        private void dgvThongBao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvThongBao.CurrentRow == null) return;

            var row = dgvThongBao.Rows[e.RowIndex];
            txtNoiDung.ForeColor = Color.Black;
            txtNoiDung.Text = row.Cells["NoiDung"]?.Value?.ToString() ?? "";

            if (int.TryParse(row.Cells["NguoiNhan"]?.Value?.ToString(), out int maNhan))
                cboNguoiNhan.SelectedValue = maNhan;
            else
                cboNguoiNhan.SelectedIndex = -1;

            if (DateTime.TryParse(row.Cells["NgayGui"]?.Value?.ToString(), out DateTime dt))
                dtNgayGui.Value = dt;
            else
                dtNgayGui.Value = DateTime.Now;
        }
    }
}
