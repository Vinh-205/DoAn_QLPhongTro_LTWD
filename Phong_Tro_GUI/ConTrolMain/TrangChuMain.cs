using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_GUI
{
    public partial class TrangChuMain : UserControl
    {
        // 👉 Khởi tạo trực tiếp như bạn yêu cầu
        private PhongBUS bus = new PhongBUS();

        public TrangChuMain()
        {
            InitializeComponent();
            HienThiSoDoPhong(); // Gọi khi control load
        }

        // ================= HIỂN THỊ SƠ ĐỒ PHÒNG =================
        private void HienThiSoDoPhong()
        {
            // Xóa control cũ và thêm lại label hành lang
            groupSoDo.Controls.Clear();
            groupSoDo.Controls.Add(lblHanhLang);

            List<Phong> danhSachPhong = bus.LayTatCa();

            if (danhSachPhong == null || danhSachPhong.Count == 0)
            {
                Label lbl = new Label()
                {
                    Text = "Chưa có dữ liệu phòng!",
                    AutoSize = true,
                    ForeColor = Color.Gray,
                    Font = new Font("Segoe UI", 10, FontStyle.Italic),
                    Location = new Point(250, 150)
                };
                groupSoDo.Controls.Add(lbl);
                return;
            }

            // Sắp xếp danh sách phòng theo tên
            danhSachPhong = danhSachPhong.OrderBy(p => p.TenPhong).ToList();

            // Layout
            int phongMoiHang = 5;
            int startX = 60, startY = 50;
            int spacingX = 110, spacingY = 120;
            int btnWidth = 90, btnHeight = 50;

            for (int i = 0; i < danhSachPhong.Count; i++)
            {
                Phong p = danhSachPhong[i];

                Button btn = new Button()
                {
                    Text = p.TenPhong,
                    Width = btnWidth,
                    Height = btnHeight,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10),
                    Tag = p,
                    BackColor = LayMauTheoTrangThai(p.TrangThai),
                };
                btn.FlatAppearance.BorderColor = Color.Gray;
                btn.FlatAppearance.BorderSize = 1;

                // Tính vị trí
                int row = i / phongMoiHang;
                int col = i % phongMoiHang;
                btn.Location = new Point(startX + col * spacingX, startY + row * spacingY + (row > 0 ? 30 : 0));

                // Gán sự kiện click
                btn.Click += Btn_Click;

                groupSoDo.Controls.Add(btn);
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is Phong phong)
            {
                string thongTin = $"Mã phòng: {phong.MaPhong}\n" +
                                  $"Tên phòng: {phong.TenPhong}\n" +
                                  $"Trạng thái: {phong.TrangThai}\n" +
                                  $"Giá phòng: {phong.GiaPhong:N0} VNĐ\n" +
                                  $"Mô tả: {phong.MoTa}";
                MessageBox.Show(thongTin, "Thông tin phòng", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private Color LayMauTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                return Color.LightGray;

            switch (trangThai.Trim().ToLower())
            {
                case "trống":
                case "con trong":
                    return Color.LightGreen;
                case "đang thuê":
                case "da thue":
                    return Color.LightCoral;
                case "đang sửa":
                case "sua chua":
                    return Color.Khaki;
                default:
                    return Color.WhiteSmoke;
            }
        }

        // Giải phóng tài nguyên
       
    }
}
