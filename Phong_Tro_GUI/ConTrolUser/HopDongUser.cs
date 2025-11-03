using System;
using System.Linq;
using System.Windows.Forms;
using Phong_Tro_BUS;

namespace Phong_Tro_GUI.ConTrolUser
{
    public partial class HopDongUser : UserControl
    {
        private HopDongBUS hopDongBUS;
        private string maNguoiThue;

        public HopDongUser(string maNguoiThue)
        {
            InitializeComponent();
            this.maNguoiThue = maNguoiThue;
            hopDongBUS = new HopDongBUS();
            this.Load += HopDongUser_Load;
        }

        private void HopDongUser_Load(object sender, EventArgs e)
        {
            LoadHopDong();
            txtTimKiem.TextChanged += TxtTimKiem_TextChanged;
        }
        private void LoadHopDong(string keyword = "")
        {
            try
            {
                int maNguoiThueInt = int.Parse(maNguoiThue);
                var dsHopDong = hopDongBUS.LayHopDongTheoNguoiThue(maNguoiThueInt);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = keyword.ToLower();
                    dsHopDong = dsHopDong
                        .Where(hd =>
                            hd.MaHopDong.ToString().ToLower().Contains(keyword) ||
                            hd.TenPhong.ToLower().Contains(keyword))
                        .ToList();
                }

                dgvHopDong.DataSource = dsHopDong;

                dgvHopDong.Columns["MaHopDong"].HeaderText = "Mã hợp đồng";
                dgvHopDong.Columns["TenPhong"].HeaderText = "Phòng";
                dgvHopDong.Columns["NgayBatDau"].HeaderText = "Ngày bắt đầu";
                dgvHopDong.Columns["NgayKetThuc"].HeaderText = "Ngày kết thúc";
                dgvHopDong.Columns["TienThue"].HeaderText = "Tiền thuê (VNĐ)";
                dgvHopDong.Columns["TrangThai"].HeaderText = "Trạng thái";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách hợp đồng: " + ex.Message);
            }
        }
        private void TxtTimKiem_TextChanged(object sender, EventArgs e)
        {
            LoadHopDong(txtTimKiem.Text);
        }
    }
}
