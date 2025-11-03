using System;
using System.Windows.Forms;
using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_GUI.ConTrolUser
{
    public partial class ThongTinUser : UserControl
    {
        private readonly KhachThueBUS _khachThueBUS;
        private KhachThue _khachThueHienTai;

        public ThongTinUser(int maKhach)
        {
            InitializeComponent();
            _khachThueBUS = new KhachThueBUS();
            TaiThongTin(maKhach);
        }

        private void TaiThongTin(int maKhach)
        {
            try
            {
                _khachThueHienTai = _khachThueBUS.LayTheoMa(maKhach);

                if (_khachThueHienTai == null)
                {
                    MessageBox.Show("❌ Không tìm thấy thông tin khách thuê!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🧩 Nạp thông tin từ DB vào label
                lblTen.Text = "👤 Tên: " + (_khachThueHienTai.Ten ?? "Chưa cập nhật");
                lblSDT.Text = "📞 SĐT: " + (_khachThueHienTai.SDT ?? "Chưa cập nhật");
                lblEmail.Text = "📧 Email: " + (_khachThueHienTai.Email ?? "Chưa cập nhật");
                lblCCCD.Text = "🪪 CCCD: " + (_khachThueHienTai.CCCD ?? "Chưa cập nhật");
                lblNgaySinh.Text = "🎂 Ngày sinh: " + (_khachThueHienTai.NgaySinh?.ToString("dd/MM/yyyy") ?? "Chưa cập nhật");
                lblDiaChi.Text = "📍 Địa chỉ: " + (_khachThueHienTai.DiaChi ?? "Chưa cập nhật");
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Lỗi khi tải thông tin khách thuê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
