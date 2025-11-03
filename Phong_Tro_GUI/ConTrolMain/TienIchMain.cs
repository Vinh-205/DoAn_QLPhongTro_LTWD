using System;
using System.Linq;
using System.Windows.Forms;
using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro; 


namespace Phong_Tro_GUI
{
    public partial class TienIchMain : UserControl
    {
        private readonly TienIchBUS _bus = new TienIchBUS();

        public TienIchMain()
        {
            InitializeComponent();
            this.Load += TienIchMain_Load;
            dgvDichVu.CellClick += dgvDichVu_CellClick; // đúng event
        }

        private void TienIchMain_Load(object sender, EventArgs e)
        {
            LoadDanhSach();
            ClearFields();
        }

        // ==================== LOAD DỮ LIỆU ====================
        private void LoadDanhSach()
        {
            var list = _bus.LayTatCa()
                .Select(ti => new
                {
                    ti.MaTienIch,
                    ti.TenTienIch,
                    ti.DonGia,
                    ti.MoTa
                }).ToList();

            dgvDichVu.DataSource = list;
            dgvDichVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDichVu.ClearSelection();
        }

        // ==================== LÀM MỚI Ô NHẬP ====================
        private void ClearFields()
        {
            txtMaDV.Clear();
            txtTenDV.Clear();
            txtDonGia.Clear();
            txtMoTa.Clear();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            ClearFields();
            LoadDanhSach();
        }

        // ==================== THÊM ====================
        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTenDV.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên tiện ích!");
                    return;
                }

                var ti = new TienIch
                {
                    TenTienIch = txtTenDV.Text.Trim(),
                    DonGia = decimal.TryParse(txtDonGia.Text, out decimal gia) ? gia : 0,
                    MoTa = txtMoTa.Text.Trim()
                };

                _bus.Them(ti);
                MessageBox.Show("Thêm tiện ích thành công!", "Thông báo");
                LoadDanhSach();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm tiện ích: " + ex.Message);
            }
        }

        // ==================== SỬA ====================
        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaDV.Text))
                {
                    MessageBox.Show("Vui lòng chọn tiện ích cần sửa!");
                    return;
                }

                var ti = new TienIch
                {
                    MaTienIch = Convert.ToInt32(txtMaDV.Text),
                    TenTienIch = txtTenDV.Text.Trim(),
                    DonGia = decimal.TryParse(txtDonGia.Text, out decimal gia) ? gia : 0,
                    MoTa = txtMoTa.Text.Trim()
                };

                _bus.Sua(ti);
                MessageBox.Show("Cập nhật tiện ích thành công!", "Thông báo");
                LoadDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sửa tiện ích: " + ex.Message);
            }
        }

        // ==================== XÓA ====================
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMaDV.Text))
                {
                    MessageBox.Show("Vui lòng chọn tiện ích cần xóa!");
                    return;
                }

                int ma = Convert.ToInt32(txtMaDV.Text);
                if (MessageBox.Show("Bạn có chắc muốn xóa tiện ích này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _bus.Xoa(ma);
                    MessageBox.Show("Xóa tiện ích thành công!", "Thông báo");
                    LoadDanhSach();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa tiện ích: " + ex.Message);
            }
        }

        // ==================== CLICK DÒNG DỮ LIỆU ====================
        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvDichVu.Rows.Count > 0)
            {
                var row = dgvDichVu.Rows[e.RowIndex];
                txtMaDV.Text = row.Cells["MaTienIch"].Value?.ToString();
                txtTenDV.Text = row.Cells["TenTienIch"].Value?.ToString();
                txtDonGia.Text = row.Cells["DonGia"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();
            }
        }
    }
}
