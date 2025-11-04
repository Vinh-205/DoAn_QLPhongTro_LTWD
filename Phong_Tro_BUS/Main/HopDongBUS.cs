using Phong_Tro_DAL.PhongTro;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Phong_Tro_BUS
{
    public class HopDongBUS : IDisposable
    {
        private readonly Connect db;

        public HopDongBUS()
        {
            db = new Connect();
        }

        // ======== LẤY TẤT CẢ HỢP ĐỒNG ========
        public List<HopDong> LayTatCa()
        {
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .Include(h => h.HoaDons)
                .AsNoTracking()
                .OrderByDescending(h => h.NgayBatDau)
                .ToList();
        }

        // ======== LẤY THEO MÃ ========
        public HopDong LayTheoMa(int maHopDong)
        {
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .Include(h => h.HoaDons)
                .AsNoTracking()
                .FirstOrDefault(h => h.MaHopDong == maHopDong);
        }

        // ======== LẤY HỢP ĐỒNG THEO PHÒNG ========
        public HopDong LayHopDongTheoPhong(string maPhong)
        {
            return db.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.KhachThue)
                .Include(h => h.HoaDons)
                .AsNoTracking()
                .FirstOrDefault(h => h.MaPhong == maPhong);
        }

        // ======== THÊM ========
        public bool Them(HopDong hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            // Kiểm tra phòng đã có hợp đồng hay chưa
            bool tonTai = db.HopDongs.Any(h => h.MaPhong == hd.MaPhong);
            if (tonTai)
                throw new Exception($"Phòng {hd.MaPhong} đã có hợp đồng!");

            hd.NgayBatDau = hd.NgayBatDau == default ? DateTime.Now : hd.NgayBatDau;

            db.HopDongs.Add(hd);
            db.SaveChanges();
            return true;
        }

        // ======== SỬA ========
        public bool Sua(HopDong hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            var old = db.HopDongs.Find(hd.MaHopDong);
            if (old == null)
                throw new Exception("Không tìm thấy hợp đồng để sửa!");

            db.Entry(old).CurrentValues.SetValues(hd);
            db.SaveChanges();
            return true;
        }

        // ======== XÓA ========
        public bool Xoa(int maHopDong)
        {
            var hd = db.HopDongs.Find(maHopDong);
            if (hd == null)
                throw new Exception("Không tìm thấy hợp đồng!");

            db.HopDongs.Remove(hd);
            db.SaveChanges();
            return true;
        }

        // ======== TÌM KIẾM ========
        public List<HopDong> TimKiem(string tuKhoa)
        {
            tuKhoa = tuKhoa?.Trim().ToLower();

            var query = db.HopDongs
                .Include(h => h.KhachThue)
                .Include(h => h.Phong)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tuKhoa))
            {
                query = query.Where(h =>
                    h.Phong.TenPhong.ToLower().Contains(tuKhoa) ||
                    h.KhachThue.Ten.ToLower().Contains(tuKhoa));
            }

            return query.AsNoTracking().ToList();
        }

        // ======== KẾT THÚC HỢP ĐỒNG ========
        public bool KetThucHopDong(int maHopDong)
        {
            var hd = db.HopDongs.Find(maHopDong);
            if (hd == null)
                throw new Exception("Không tìm thấy hợp đồng!");

            hd.NgayKetThuc = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        // ======== LẤY HỢP ĐỒNG THEO NGƯỜI THUÊ ========
        public List<HopDongView> LayHopDongTheoNguoiThue(int maNguoiThue)
        {
            return db.HopDongs
                .Include(hd => hd.Phong)
                .Include(hd => hd.KhachThue)
                .Where(hd => hd.MaKhach == maNguoiThue)
                .Select(hd => new HopDongView
                {
                    MaHopDong = hd.MaHopDong,
                    TenPhong = hd.Phong != null ? hd.Phong.TenPhong : "Chưa có",
                    NgayBatDau = hd.NgayBatDau,
                    NgayKetThuc = hd.NgayKetThuc,
                    TienThue = hd.TienThue
                })
                .ToList();
        }

        // ======== GIẢI PHÓNG TÀI NGUYÊN ========
        public void Dispose()
        {
            db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    // ======== VIEW MODEL ========
    public class HopDongView
    {
        public int MaHopDong { get; set; }
        public string TenPhong { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public decimal? TienThue { get; set; }
    }
}
