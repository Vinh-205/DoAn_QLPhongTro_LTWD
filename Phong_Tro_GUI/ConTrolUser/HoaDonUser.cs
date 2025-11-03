using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_GUI.ConTrolUser
{
    public partial class HoaDonUser : UserControl
    {
        private readonly HoaDonBUS hoaDonBUS;
        private readonly int maKhachHienTai;

        // === Constructor runtime: bắt buộc truyền maKhach ===
        public HoaDonUser(int maKhach)
        {
            InitializeComponent();
            hoaDonBUS = new HoaDonBUS();
            maKhachHienTai = maKhach;
            this.Load += HoaDonUser_Load;
        }

        // === Constructor parameterless cho Designer ===
        public HoaDonUser()
        {
            InitializeComponent();
            hoaDonBUS = new HoaDonBUS();
            maKhachHienTai = 0; // mặc định 0, sẽ không load dữ liệu
        }

        private void HoaDonUser_Load(object sender, EventArgs e)
        {
            SetupSearchBox();
            SetupGridView();

            if (maKhachHienTai > 0)
                LoadHoaDon();
        }

        // === Placeholder cho ô tìm kiếm ===
        private void SetupSearchBox()
        {
            txtTimKiem.Text = "Nhập mã hóa đơn hoặc tên phòng...";
            txtTimKiem.ForeColor = Color.Gray;

            txtTimKiem.Enter += (s, e) =>
            {
                if (txtTimKiem.Text == "Nhập mã hóa đơn hoặc tên phòng...")
                {
                    txtTimKiem.Text = "";
                    txtTimKiem.ForeColor = Color.Black;
                }
            };

            txtTimKiem.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtTimKiem.Text))
                {
                    txtTimKiem.Text = "Nhập mã hóa đơn hoặc tên phòng...";
                    txtTimKiem.ForeColor = Color.Gray;
                }
            };

            txtTimKiem.TextChanged += txtTimKiem_TextChanged;
        }

        // === Thiết lập DataGridView ===
        private void SetupGridView()
        {
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.MultiSelect = false;
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.EnableHeadersVisualStyles = false;

            dgvHoaDon.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
            dgvHoaDon.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHoaDon.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            dgvHoaDon.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvHoaDon.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        }

        // === Load danh sách hóa đơn theo khách ===
        private void LoadHoaDon()
        {
            try
            {
                var list = hoaDonBUS.LayTheoKhach(maKhachHienTai)
                    .Select(hd => new
                    {
                        hd.MaHD,
                        Phong = hd.HopDong.Phong.TenPhong,
                        hd.Thang,
                        hd.Nam,
                        NgayLap = hd.NgayLap?.ToString("dd/MM/yyyy"),
                        hd.TongTien
                    })
                    .ToList();

                dgvHoaDon.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // === Tìm kiếm hóa đơn theo từ khóa ===
        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "Nhập mã hóa đơn hoặc tên phòng...") return;
            if (maKhachHienTai <= 0) return;

            string keyword = txtTimKiem.Text.Trim().ToLower();

            var data = hoaDonBUS.LayTheoKhach(maKhachHienTai)
                .Where(hd =>
                    hd.MaHD.ToLower().Contains(keyword)
                    || hd.HopDong.Phong.TenPhong.ToLower().Contains(keyword))
                .Select(hd => new
                {
                    hd.MaHD,
                    Phong = hd.HopDong.Phong.TenPhong,
                    hd.Thang,
                    hd.Nam,
                    NgayLap = hd.NgayLap?.ToString("dd/MM/yyyy"),
                    hd.TongTien
                })
                .ToList();

            dgvHoaDon.DataSource = data;
        }
    }
}
