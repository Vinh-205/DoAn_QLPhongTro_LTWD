using Phong_Tro_DAL.PhongTro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phong_Tro_BUS
{
    public class HoaDonBUS : IDisposable
    {
        private readonly Connect connect;

        public HoaDonBUS()
        {
            connect = new Connect();
        }

        public List<HoaDon> GetAll()
        {
            try
            {
                return connect.HoaDons
                    .AsNoTracking()               
                    .ToList();
            }
            catch (Exception)
            {
                return new List<HoaDon>();
            }
        }

        public HoaDon GetById(string maHD)
        {
            return connect.HoaDons.FirstOrDefault(h => h.MaHD == maHD);
        }

        public bool Add(HoaDon hd)
        {
            try
            {
                connect.HoaDons.Add(hd);
                connect.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Update(HoaDon hd)
        {
            try
            {
                var existing = GetById(hd.MaHD);
                if (existing == null) return false;

                existing.MaHopDong = hd.MaHopDong;
                existing.NgayLap = hd.NgayLap;
                existing.GiaPhong = hd.GiaPhong;
                existing.TienDien = hd.TienDien;
                existing.TienNuoc = hd.TienNuoc;
                existing.TienDichVu = hd.TienDichVu;
                existing.TrangThai = hd.TrangThai;

                connect.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Delete(string maHD)
        {
            try
            {
                var hd = GetById(maHD);
                if (hd == null) return false;

                connect.HoaDons.Remove(hd);
                connect.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        // Lấy hóa đơn theo khách thuê
        public List<HoaDon> LayTheoKhach(int maKhach)
        {
            return connect.HoaDons
                .Where(h => h.HopDong != null &&
                            h.HopDong.KhachThue != null &&
                            h.HopDong.KhachThue.MaKhach == maKhach)
                .ToList();
        }

        // ======= Implement IDisposable =======
        public void Dispose()
        {
            connect?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
