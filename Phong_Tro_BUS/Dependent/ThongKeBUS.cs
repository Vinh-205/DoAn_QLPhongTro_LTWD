using System;
using System.Collections.Generic;
using System.Linq;
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

        // Lấy danh sách phòng
        public List<PhongDTO> LayDanhSachPhong()
        {
            return db.Phongs
                     .Select(p => new PhongDTO
                     {
                         MaPhong = p.MaPhong,
                         TenPhong = p.TenPhong
                     })
                     .ToList();
        }

        // Thống kê doanh thu theo phòng
        public List<DoanhThuPhong> DoanhThuTheoPhong(int thang, int nam, string maPhong = null)
        {
            var hoaDonList = db.HoaDons
                               .Where(hd => hd.Thang == thang && hd.Nam == nam)
                               .ToList();

            // Lọc theo mã phòng nếu có
            if (!string.IsNullOrEmpty(maPhong))
            {
                hoaDonList = hoaDonList.Where(hd => hd.HopDong.MaPhong == maPhong).ToList();
            }

            return hoaDonList
                .GroupBy(hd => hd.HopDong.MaPhong)
                .Select(g => new DoanhThuPhong
                {
                    MaPhong = g.Key,
                    TongTien = g.Sum(hd => hd.TongTien ?? 0)
                })
                .OrderByDescending(x => x.TongTien)
                .ToList();
        }

        // Thống kê doanh thu dịch vụ
        public List<DoanhThuDichVu> DoanhThuDichVuTheoThang(int thang, int nam)
        {
            var chiTietList = db.ChiTietHoaDons
                                .Where(ct => ct.HoaDon.Thang == thang && ct.HoaDon.Nam == nam)
                                .ToList();

            return chiTietList
                .GroupBy(ct => new { ct.MaDV, ct.DichVu.TenDV })
                .Select(g => new DoanhThuDichVu
                {
                    MaDV = g.Key.MaDV,
                    TenDV = g.Key.TenDV,
                    TongTien = g.Sum(ct => ct.ThanhTien ?? 0)
                })
                .OrderByDescending(x => x.TongTien)
                .ToList();
        }

        // Giải phóng tài nguyên
        public void Dispose()
        {
            db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

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
