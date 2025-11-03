using System;
using System.Linq;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_BUS
{
    public class TaiKhoanBUS
    {
        // Đăng nhập
        public TaiKhoan KiemTraDangNhap(string username, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Thiếu thông tin đăng nhập.");

            using (var db = new Connect())
            {
                return db.TaiKhoans
                         .FirstOrDefault(t =>
                             t.TenDangNhap == username &&
                             t.MatKhau == password &&
                             t.LoaiTK == role);
            }
        }

        // Lấy khách thuê tương ứng tài khoản
        public KhachThue LayKhachTheoTaiKhoan(TaiKhoan tk)
        {
            if (tk == null)
                throw new ArgumentNullException(nameof(tk));

            using (var db = new Connect())
            {
                return db.KhachThues.FirstOrDefault(k => k.Email == tk.Email);
            }
        }

        // Thêm tài khoản
        public bool Them(TaiKhoan tk)
        {
            if (tk == null) throw new ArgumentNullException(nameof(tk));

            using (var db = new Connect())
            {
                if (db.TaiKhoans.Any(x => x.TenDangNhap == tk.TenDangNhap))
                    throw new Exception("Tên đăng nhập đã tồn tại.");

                tk.NgayTao = DateTime.Now;
                db.TaiKhoans.Add(tk);
                db.SaveChanges();
                return true;
            }
        }

        // Cập nhật tài khoản
        public bool Sua(TaiKhoan tk)
        {
            if (tk == null) throw new ArgumentNullException(nameof(tk));

            using (var db = new Connect())
            {
                var existing = db.TaiKhoans.Find(tk.MaTK);
                if (existing == null)
                    throw new Exception("Không tìm thấy tài khoản.");

                existing.MatKhau = tk.MatKhau;
                existing.HoTen = tk.HoTen;
                existing.Email = tk.Email;
                existing.SDT = tk.SDT;
                existing.LoaiTK = tk.LoaiTK;

                db.SaveChanges();
                return true;
            }
        }

        // Xóa tài khoản
        public bool Xoa(int maTK)
        {
            using (var db = new Connect())
            {
                var tk = db.TaiKhoans.Find(maTK);
                if (tk == null)
                    throw new Exception("Không tìm thấy tài khoản để xóa.");

                db.TaiKhoans.Remove(tk);
                db.SaveChanges();
                return true;
            }
        }

        // ==================== Lấy lại mật khẩu ====================
        public TaiKhoan LayLaiMatKhau(string tenDangNhap, string email)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(email))
                return null;

            using (var db = new Connect())
            {
                return db.TaiKhoans
                         .FirstOrDefault(tk =>
                             tk.TenDangNhap.Equals(tenDangNhap, StringComparison.OrdinalIgnoreCase) &&
                             tk.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
