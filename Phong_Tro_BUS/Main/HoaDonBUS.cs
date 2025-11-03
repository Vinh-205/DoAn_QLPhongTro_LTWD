using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_BUS
{
    public class HoaDonBUS
    {
        private readonly Connect db;

        public HoaDonBUS()
        {
            db = new Connect();
        }

        // ===========================
        // QUYỀN NGƯỜI THUÊ (CHỈ XEM)
        // ===========================
        public List<HoaDon> LayTatCa()
        {
            return db.HoaDons
                     .Include(hd => hd.HopDong)
                     .Include(hd => hd.HopDong.Phong)
                     .Include(hd => hd.HopDong.KhachThue)
                     .Include(hd => hd.ChiTietHoaDons.Select(ct => ct.DichVu))
                     .AsNoTracking()
                     .OrderByDescending(hd => hd.NgayLap)
                     .ToList();
        }

        public List<HoaDon> LayTheoKhach(int maKhach)
        {
            return db.HoaDons
                     .Include(hd => hd.HopDong)
                     .Include(hd => hd.HopDong.Phong)
                     .Include(hd => hd.HopDong.KhachThue)
                     .Where(hd => hd.HopDong != null
                                  && hd.HopDong.KhachThue != null
                                  && hd.HopDong.KhachThue.MaKhach == maKhach)
                     .AsNoTracking()
                     .OrderByDescending(hd => hd.NgayLap)
                     .ToList();
        }

        public HoaDon LayTheoMa(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
                throw new ArgumentException("Mã hóa đơn không hợp lệ!");

            return db.HoaDons
                     .Include(hd => hd.HopDong)
                     .Include(hd => hd.ChiTietHoaDons.Select(ct => ct.DichVu))
                     .AsNoTracking()
                     .FirstOrDefault(hd => hd.MaHD == maHD);
        }

        public List<HoaDon> TimKiem(string tuKhoa)
        {
            if (string.IsNullOrWhiteSpace(tuKhoa))
                return LayTatCa();

            tuKhoa = tuKhoa.Trim();

            return db.HoaDons
                     .Include(hd => hd.HopDong)
                     .Include(hd => hd.HopDong.KhachThue)
                     .Where(hd => hd.MaHD.Contains(tuKhoa)
                                  || hd.Thang.ToString().Contains(tuKhoa)
                                  || hd.Nam.ToString().Contains(tuKhoa))
                     .AsNoTracking()
                     .OrderByDescending(hd => hd.NgayLap)
                     .ToList();
        }

        // ===========================
        // QUYỀN CHỦ TRỌ (THÊM / SỬA / XÓA)
        // ===========================
        public bool Them(HoaDon hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            // Dùng Any() cần using System.Linq
            if (db.HoaDons.Any(x => x.MaHD == hd.MaHD))
                throw new Exception("❌ Mã hóa đơn đã tồn tại!");

            hd.TongTien = TinhTongTien(hd);
            hd.NgayLap = DateTime.Now;

            db.HoaDons.Add(hd);
            db.SaveChanges();
            return true;
        }

        public bool Sua(HoaDon hd)
        {
            if (hd == null)
                throw new ArgumentNullException(nameof(hd));

            var existing = db.HoaDons.Find(hd.MaHD);
            if (existing == null)
                throw new Exception("❌ Không tìm thấy hóa đơn để cập nhật!");

            existing.MaHopDong = hd.MaHopDong;
            existing.Thang = hd.Thang;
            existing.Nam = hd.Nam;
            existing.SoDienCu = hd.SoDienCu;
            existing.SoDienMoi = hd.SoDienMoi;
            existing.SoNuocCu = hd.SoNuocCu;
            existing.SoNuocMoi = hd.SoNuocMoi;
            existing.TienDien = hd.TienDien;
            existing.TienNuoc = hd.TienNuoc;
            existing.TienDichVu = hd.TienDichVu;
            existing.GiaPhong = hd.GiaPhong;
            existing.TongTien = TinhTongTien(hd);
            existing.NgayLap = hd.NgayLap ?? DateTime.Now;

            db.Entry(existing).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool Xoa(string maHD)
        {
            var hd = db.HoaDons.Find(maHD);
            if (hd == null)
                throw new Exception("❌ Không tìm thấy hóa đơn để xóa!");

            db.HoaDons.Remove(hd);
            db.SaveChanges();
            return true;
        }

        // ===========================
        // HELPER
        // ===========================
        public decimal TinhTongTien(HoaDon hd)
        {
            decimal tienDichVu = hd.TienDichVu ?? 0;
            decimal tienDien = hd.TienDien ?? 0;
            decimal tienNuoc = hd.TienNuoc ?? 0;
            decimal giaPhong = hd.GiaPhong ?? 0;

            return tienDichVu + tienDien + tienNuoc + giaPhong;
        }

        public decimal TinhTongDoanhThu(int thang, int nam)
        {
            return db.HoaDons
                     .Where(hd => hd.Thang == thang && hd.Nam == nam)
                     .Sum(hd => (decimal?)hd.TongTien ?? 0);
        }
    }
}
