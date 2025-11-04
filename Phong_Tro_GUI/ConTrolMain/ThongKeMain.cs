using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Phong_Tro_GUI
{
    public partial class ThongKeMain : UserControl
    {
        private ThongKeBUS thongKeBUS;

        public ThongKeMain()
        {
            InitializeComponent();
            thongKeBUS = new ThongKeBUS();
            LoadCboNamThang();
            LoadDanhSachPhong();
        }

        private void LoadCboNamThang()
        {
            // Tháng 1 -> 12
            cboThang.Items.Clear();
            for (int i = 1; i <= 12; i++) cboThang.Items.Add(i);
            cboThang.SelectedIndex = 0;

            // Năm: lấy 5 năm gần nhất
            cboNam.Items.Clear();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 4; i <= currentYear; i++) cboNam.Items.Add(i);
            cboNam.SelectedIndex = 4;
        }

        private void LoadDanhSachPhong()
        {
            var phongList = thongKeBUS.LayDanhSachPhong();
            cboPhong.Items.Clear();
            cboPhong.Items.Add("Tất cả");
            cboPhong.Items.AddRange(phongList.Select(p => p.TenPhong).ToArray());
            cboPhong.SelectedIndex = 0;
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            int thang = Convert.ToInt32(cboThang.SelectedItem);
            int nam = Convert.ToInt32(cboNam.SelectedItem);
            string tenPhong = cboPhong.SelectedItem.ToString();
            string maPhong = null;

            if (tenPhong != "Tất cả")
            {
                maPhong = thongKeBUS.LayDanhSachPhong()
                                    .FirstOrDefault(p => p.TenPhong == tenPhong)?.MaPhong;
            }

            // Tổng hợp
            var tongHop = thongKeBUS.TongHopDoanhThu(thang, nam, maPhong);
            txtSoHD.Text = tongHop.TongSoHoaDon.ToString();
            txtTongTien.Text = tongHop.TongDoanhThu?.ToString("N0");
            txtTBHoaDon.Text = (tongHop.TongSoHoaDon.HasValue && tongHop.TongSoHoaDon.Value > 0)
                 ? ((decimal)(tongHop.TongDoanhThu ?? 0) / tongHop.TongSoHoaDon.Value).ToString("N0")
                  : "0";


            // DataGridView doanh thu dịch vụ
            dgvDoanhThuDichVu.DataSource = thongKeBUS.DoanhThuDichVuTheoThang(thang, nam, maPhong);

            // Chart Pie
            var dsChart = thongKeBUS.DoanhThuDichVuTheoThang(thang, nam, maPhong);
            chart1.Series.Clear();
            var series = new Series
            {
                Name = "DoanhThu",
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Pie
            };
            chart1.Series.Add(series);
            chart1.Legends[0].Docking = Docking.Bottom;

            foreach (var item in dsChart)
            {
                series.Points.AddXY(item.TenDV, item.TongTien);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            cboThang.SelectedIndex = 0;
            cboNam.SelectedIndex = cboNam.Items.Count - 1;
            cboPhong.SelectedIndex = 0;
            dgvDoanhThuDichVu.DataSource = null;
            txtSoHD.Text = "";
            txtTongTien.Text = "";
            txtTBHoaDon.Text = "";
            chart1.Series.Clear();
        }

        // Xuất Excel / PDF có thể thêm sau
    }
}
