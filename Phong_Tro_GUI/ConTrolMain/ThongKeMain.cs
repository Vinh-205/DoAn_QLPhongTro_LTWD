using Phong_Tro_BUS;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Phong_Tro_GUI
{
    public partial class ThongKeMain : UserControl
    {
        private readonly ThongKeBUS thongKeBUS = new ThongKeBUS();

        public ThongKeMain()
        {
            InitializeComponent();
            LoadComboBox();
        }

        private void LoadComboBox()
        {
            // Tháng
            cboThang.Items.Clear();
            for (int i = 1; i <= 12; i++)
                cboThang.Items.Add(i);

            // Năm
            cboNam.Items.Clear();
            for (int i = 2020; i <= DateTime.Now.Year; i++)
                cboNam.Items.Add(i);

            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            cboNam.SelectedIndex = cboNam.Items.Count - 1;

            // Phòng
            var phongList = thongKeBUS.LayDanhSachPhong();
            cboPhong.DataSource = phongList;
            cboPhong.DisplayMember = "TenPhong";
            cboPhong.ValueMember = "MaPhong";
            cboPhong.SelectedIndex = -1;
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboThang.SelectedItem == null || cboNam.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn tháng và năm!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int thang = Convert.ToInt32(cboThang.SelectedItem);
                int nam = Convert.ToInt32(cboNam.SelectedItem);
                string maPhong = cboPhong.SelectedValue?.ToString();

                // === Load doanh thu phòng ===
                var hoaDonList = thongKeBUS.DoanhThuTheoPhong(thang, nam);
                if (!string.IsNullOrEmpty(maPhong))
                    hoaDonList = hoaDonList.Where(x => x.MaPhong == maPhong).ToList();

                dgvHoaDon.DataSource = hoaDonList;
                dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvHoaDon.ClearSelection();

                // === Load doanh thu dịch vụ ===
                var dvList = thongKeBUS.DoanhThuDichVuTheoThang(thang, nam);
                dgvDoanhThuDichVu.DataSource = dvList;
                dgvDoanhThuDichVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDoanhThuDichVu.ClearSelection();

                // === Tính tổng ===
                txtSoHD.Text = hoaDonList.Count.ToString();
                txtTongTien.Text = hoaDonList.Sum(x => x.TongTien).ToString("N0");
                txtTBHoaDon.Text = hoaDonList.Count > 0
                    ? (hoaDonList.Sum(x => x.TongTien) / hoaDonList.Count).ToString("N0")
                    : "0";

                // === Biểu đồ ===
                chart1.Series.Clear();
                Series series = new Series("DoanhThuPhòng")
                {
                    ChartType = SeriesChartType.Pie
                };
                foreach (var item in hoaDonList)
                {
                    series.Points.AddXY(item.MaPhong, item.TongTien);
                }

                chart1.Series.Add(series);
                chart1.Legends[0].Docking = Docking.Bottom;
                chart1.Legends[0].Alignment = StringAlignment.Center;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            dgvHoaDon.DataSource = null;
            dgvDoanhThuDichVu.DataSource = null;
            txtSoHD.Clear();
            txtTongTien.Clear();
            txtTBHoaDon.Clear();
            chart1.Series.Clear();
            cboThang.SelectedIndex = DateTime.Now.Month - 1;
            cboNam.SelectedIndex = cboNam.Items.Count - 1;
            cboPhong.SelectedIndex = -1;
        }
    }
}
