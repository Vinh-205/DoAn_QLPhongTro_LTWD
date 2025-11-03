using System;
using System.Windows.Forms;
using Phong_Tro_BUS;
using Phong_Tro_DAL.PhongTro;
using Phong_Tro_GUI.ConTrolMain;
using Phong_Tro_GUI.ConTrolUser;

namespace Phong_Tro_GUI
{
    public partial class DangNhap : Form
    {
        private readonly TaiKhoanBUS taiKhoanBUS = new TaiKhoanBUS();

        public DangNhap()
        {
            InitializeComponent();
        }

        private void DangNhap_Load(object sender, EventArgs e)
        {
            cboRole.Items.AddRange(new string[] { "Admin", "User" });
            cboRole.SelectedIndex = 0;
            txtPassword.UseSystemPasswordChar = true;
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = cboRole.SelectedItem.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                TaiKhoan tk = taiKhoanBUS.KiemTraDangNhap(username, password, role);

                if (tk == null)
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi đăng nhập",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Đăng nhập thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                Hide();

                Form mainForm = new Form
                {
                    Text = role == "Admin" ? "Chủ trọ - Quản lý phòng" : "Người thuê - Giao diện người dùng",
                    WindowState = FormWindowState.Maximized,
                    BackColor = System.Drawing.Color.White
                };

                if (role == "Admin")
                {
                    var ucChuTro = new ChuTroMain();
                    ucChuTro.Dock = DockStyle.Fill;
                    mainForm.Controls.Add(ucChuTro);
                }
                else
                {
                    // Lấy thông tin khách thuê tương ứng tài khoản
                    var khachThueBUS = new KhachThueBUS();
                    var khach = taiKhoanBUS.LayKhachTheoTaiKhoan(tk);

                    if (khach == null)
                    {
                        MessageBox.Show("Không tìm thấy thông tin khách thuê!", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Show();
                        return;
                    }

                    var ucNguoiThue = new NguoiThueUser(khach.MaKhach);
                    ucNguoiThue.Dock = DockStyle.Fill;
                    mainForm.Controls.Add(ucNguoiThue);
                }

                mainForm.FormClosed += (s, args) => Application.Exit();
                mainForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void labelQuenMK_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new QuenMatKhau();
            frm.FormClosed += (s, args) => Show();
            frm.Show();
        }
    }
}
