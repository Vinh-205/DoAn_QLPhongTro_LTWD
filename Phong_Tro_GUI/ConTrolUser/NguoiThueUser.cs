using System;
using System.Drawing;
using System.Windows.Forms;
using Phong_Tro_GUI.ConTrolUser;
using Phong_Tro_GUI.ConTrolMain;

namespace Phong_Tro_GUI
{
    public partial class NguoiThueUser : UserControl
    {
        private Panel activeIndicator;
        private int maKhachHienTai;

        public NguoiThueUser(int maKhach)
        {
            InitializeComponent();
            maKhachHienTai = maKhach;
            InitializeMenuEffects();
            HighlightButton(btnTrangChu);
            LoadControl(new ThongTinUser(maKhachHienTai));
        }

        private void InitializeMenuEffects()
        {
            activeIndicator = new Panel
            {
                BackColor = Color.MidnightBlue,
                Size = new Size(6, 38)
            };
            pnlSidebar.Controls.Add(activeIndicator);
            activeIndicator.BringToFront();
        }

        private void HighlightButton(Button selectedButton)
        {
            foreach (Control ctrl in pnlSidebar.Controls)
            {
                if (ctrl is Button btn && btn != btnMenu && btn != btnDangXuat)
                {
                    btn.BackColor = Color.SteelBlue;
                    btn.ForeColor = Color.White;
                }
            }

            selectedButton.BackColor = Color.MidnightBlue;
            selectedButton.ForeColor = Color.WhiteSmoke;
            activeIndicator.Location = new Point(0, selectedButton.Top);
        }

        private void LoadControl(Control ctrl)
        {
            pnlContent.Controls.Clear();
            ctrl.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(ctrl);
        }

        private void LoadContent(string text)
        {
            pnlContent.Controls.Clear();
            Label lbl = new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.MidnightBlue,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlContent.Controls.Add(lbl);
        }

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            HighlightButton(btnTrangChu);
            LoadContent("🏠 Trang chủ người thuê");
            LoadControl(new ThongTinUser(maKhachHienTai));
        }

        private void btnPhong_Click(object sender, EventArgs e)
        {
            HighlightButton(btnPhong);
            LoadContent("🏢 Phòng đang thuê");
            LoadControl(new PhongUser(maKhachHienTai));
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            HighlightButton(btnHoaDon);
            LoadContent("💰 Hóa đơn thanh toán");
            LoadControl(new HoaDonUser());
        }

        private void btnThongBao_Click(object sender, EventArgs e)
        {
            HighlightButton(btnThongBao);
            LoadContent("💬 Thông báo từ chủ trọ");
            LoadControl(new ThongBaoUser());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            HighlightButton(btnThongKe);
            LoadContent("✍️ Gửi ý kiến cho chủ trọ");
            LoadControl(new YKienUser());
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            HighlightButton(btnDichVu);
            LoadContent("🧰 Thông tin cá nhân");
            LoadControl(new ThongTinUser(maKhachHienTai));
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?",
                                "Xác nhận",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        private void btnDangXuat_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?",
                        "Xác nhận",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Đóng toàn bộ UserControl hiện tại và quay lại form đăng nhập
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Hide(); // Ẩn form hiện tại
                    DangNhap frmDangNhap = new DangNhap();
                    frmDangNhap.Show(); // Hiển thị lại form đăng nhập
                }
            }
        }
    }
}
