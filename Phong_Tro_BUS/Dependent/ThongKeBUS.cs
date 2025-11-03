using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_BUS
{
    public class ThongKeBUS : IDisposable
    {
        private readonly Connect db;

        public ThongKeBUS()
        {
            db = new Connect();
        }

        // 1️⃣ LẤY DANH SÁCH PHÒNG
        public List<PhongDTO> LayDanhSachPhong()
        {
            return db.Phongs
                     .AsNoTracking()
                     .Select(p => new PhongDTO
                     {
                         MaPhong = p.MaPhong,
                         TenPhong = p.TenPhong
                     })
                     .ToList();
        }

        // 2️⃣ DOANH THU THEO PHÒNG
        public List<DoanhThuPhong> DoanhThuTheoPhong(int thang, int nam)
        {
            var hoaDonList = db.HoaDons
                               .Include(hd => hd.HopDong)
                               .AsNoTracking()
                               .Where(hd => hd.Thang == thang && hd.Nam == nam)
                               .ToList();

            var result = hoaDonList
                         .GroupBy(hd => hd.HopDong.MaPhong)
                         .Select(g => new DoanhThuPhong
                         {
                             MaPhong = g.Key,
                             TongTien = g.Sum(hd => hd.TongTien ?? 0)
                         })
                         .OrderByDescending(x => x.TongTien)
                         .ToList();

            return result;
        }

        // 3️⃣ DOANH THU DỊCH VỤ
        public List<DoanhThuDichVu> DoanhThuDichVuTheoThang(int thang, int nam)
        {
            var chiTietList = db.ChiTietHoaDons
                                .Include(ct => ct.HoaDon)
                                .Include(ct => ct.DichVu)
                                .AsNoTracking()
                                .Where(ct => ct.HoaDon.Thang == thang && ct.HoaDon.Nam == nam)
                                .ToList();

            var result = chiTietList
                         .GroupBy(ct => new { ct.MaDV, ct.DichVu.TenDV })
                         .Select(g => new DoanhThuDichVu
                         {
                             MaDV = g.Key.MaDV,
                             TenDV = g.Key.TenDV,
                             TongTien = g.Sum(ct => ct.ThanhTien ?? 0)
                         })
                         .OrderByDescending(x => x.TongTien)
                         .ToList();

            return result;
        }

        // 4️⃣ THỐNG KÊ TỔNG HỢP
        public ThongKeDoanhThu TongHopDoanhThu(int thang, int nam)
        {
            var doanhThuPhong = db.HoaDons
                                  .Where(hd => hd.Thang == thang && hd.Nam == nam)
                                  .Sum(hd => (decimal?)hd.TongTien) ?? 0;

            var doanhThuDichVu = db.ChiTietHoaDons
                                   .Where(ct => ct.HoaDon.Thang == thang && ct.HoaDon.Nam == nam)
                                   .Sum(ct => (decimal?)ct.ThanhTien) ?? 0;

            var tongSoHD = db.HoaDons.Count(hd => hd.Thang == thang && hd.Nam == nam);
            var tongDoanhThu = doanhThuPhong + doanhThuDichVu;

            return new ThongKeDoanhThu
            {
                Thang = thang,
                Nam = nam,
                TongSoHoaDon = tongSoHD,
                DoanhThuPhong = doanhThuPhong,
                DoanhThuDichVu = doanhThuDichVu,
                TongDoanhThu = tongDoanhThu,
                NgayCapNhat = DateTime.Now
            };
        }

        // 5️⃣ GIẢI PHÓNG
        public void Dispose()
        {
            db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    // DTO PHỤ TRỢ
    public class PhongDTO
    {
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
    }

    public class DoanhThuPhong
    {
        public string MaPhong { get; set; }
        public decimal TongTien { get; set; }
    }

    public class DoanhThuDichVu
    {
        public string MaDV { get; set; }
        public string TenDV { get; set; }
        public decimal TongTien { get; set; }
    }
}
