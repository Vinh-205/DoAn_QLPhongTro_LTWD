using System;
using System.Collections.Generic;
using System.Linq;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_BUS
{
    public class ThongKeBUS : IDisposable
    {
        private readonly Connect db;
        private readonly HoaDonBUS hoaDonBUS;
        private readonly ChiTietHoaDonBUS chiTietBUS;
        private readonly DichVuBUS dichVuBUS;

        public ThongKeBUS()
        {
            db = new Connect();
            hoaDonBUS = new HoaDonBUS();      
            chiTietBUS = new ChiTietHoaDonBUS();
            dichVuBUS = new DichVuBUS();
        }

        // ===== Lấy danh sách phòng =====
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

        // ===== Tổng hợp doanh thu =====
        public ThongKeDoanhThu TongHopDoanhThu(int thang, int nam, string maPhong = null)
        {
            var hoaDonList = hoaDonBUS.GetAll()
                                       .Where(h => h.Thang == thang && h.Nam == nam &&
                                                   (maPhong == null || h.HopDong.MaPhong == maPhong))
                                       .ToList();

            decimal tongPhong = hoaDonList.Sum(h => h.TongTien ?? 0);

            var chiTietList = chiTietBUS.LayTatCa()
                                        .Where(ct => ct.HoaDon.Thang == thang && ct.HoaDon.Nam == nam &&
                                                     (maPhong == null || ct.HoaDon.HopDong.MaPhong == maPhong))
                                        .ToList();

            decimal tongDV = chiTietList.Sum(ct => ct.ThanhTien ?? 0);

            return new ThongKeDoanhThu
            {
                Thang = thang,
                Nam = nam,
                TongSoHoaDon = hoaDonList.Count,
                DoanhThuPhong = tongPhong,
                DoanhThuDichVu = tongDV,
                TongDoanhThu = tongPhong + tongDV,
                NgayCapNhat = DateTime.Now
            };
        }

        // ===== Thống kê theo phòng =====
        public List<DoanhThuPhong> DoanhThuTheoPhong(int thang, int nam, string maPhong = null)
        {
            return hoaDonBUS.GetAll()
                            .Where(h => h.Thang == thang && h.Nam == nam &&
                                        (maPhong == null || h.HopDong.MaPhong == maPhong))
                            .GroupBy(h => h.HopDong.MaPhong)
                            .Select(g => new DoanhThuPhong
                            {
                                MaPhong = g.Key,
                                TongTien = g.Sum(h => h.TongTien ?? 0)
                            })
                            .OrderByDescending(x => x.TongTien)
                            .ToList();
        }

        // ===== Thống kê doanh thu dịch vụ =====
        public List<DoanhThuDichVu> DoanhThuDichVuTheoThang(int thang, int nam, string maPhong = null)
        {
            return chiTietBUS.LayTatCa()
                             .Where(ct => ct.HoaDon.Thang == thang && ct.HoaDon.Nam == nam &&
                                          (maPhong == null || ct.HoaDon.HopDong.MaPhong == maPhong))
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

        // ===== Giải phóng =====
        public void Dispose()
        {
            db?.Dispose();

            // Các BUS khác cần implement IDisposable, nếu không remove Dispose() ở đây
            hoaDonBUS?.DisposeSafe();
            chiTietBUS?.DisposeSafe();
            dichVuBUS?.DisposeSafe();

            GC.SuppressFinalize(this);
        }
    }

    // ===== DTOs =====
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

    public class ThongKeDoanhThu
    {
        public int Thang { get; set; }
        public int Nam { get; set; }
        public int? TongSoHoaDon { get; set; }
        public decimal? DoanhThuPhong { get; set; }
        public decimal? DoanhThuDichVu { get; set; }
        public decimal? TongDoanhThu { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }

    // Extension để dispose an toàn
    public static class DisposableExtensions
    {
        public static void DisposeSafe(this object obj)
        {
            if (obj is IDisposable d) d.Dispose();
        }
    }
}