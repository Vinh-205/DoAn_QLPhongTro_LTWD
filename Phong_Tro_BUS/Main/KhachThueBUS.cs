using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Phong_Tro_DAL.PhongTro;

namespace Phong_Tro_BUS
{
    public class KhachThueBUS
    {
        private readonly Connect db;

        public KhachThueBUS()
        {
            db = new Connect();
        }

        // Lấy tất cả khách thuê, quyền xem cho khách thuê
        public List<KhachThue> LayTatCa(bool isChinhChu)
        {
            var list = db.KhachThues
                         .Include(k => k.HopDongs)
                         .AsNoTracking()
                         .ToList();

            if (!isChinhChu)
            {
                // Khách thuê chỉ xem thông tin của chính họ
                list = list.Select(k => new KhachThue
                {
                    MaKhach = k.MaKhach,
                    Ten = k.Ten,
                    NgaySinh = k.NgaySinh,
                    DiaChi = k.DiaChi
                }).ToList();
            }
            return list;
        }

        // Thêm sửa xóa chỉ cho Chủ phòng
        public bool Them(KhachThue kh, bool isChinhChu)
        {
            if (!isChinhChu) throw new Exception("Bạn không có quyền thêm khách thuê!");
            db.KhachThues.Add(kh);
            db.SaveChanges();
            return true;
        }

        public bool Sua(KhachThue kh, bool isChinhChu)
        {
            if (!isChinhChu) throw new Exception("Bạn không có quyền sửa khách thuê!");
            var existing = db.KhachThues.Find(kh.MaKhach);
            if (existing == null) throw new Exception("Không tìm thấy khách thuê!");
            db.Entry(existing).CurrentValues.SetValues(kh);
            db.SaveChanges();
            return true;
        }

        public bool Xoa(int maKhach, bool isChinhChu)
        {
            if (!isChinhChu) throw new Exception("Bạn không có quyền xóa khách thuê!");
            var kh = db.KhachThues.Find(maKhach);
            if (kh == null) throw new Exception("Không tìm thấy khách thuê!");
            db.KhachThues.Remove(kh);
            db.SaveChanges();
            return true;
        }
        public KhachThue LayTheoMa(int maKhach)
        {
            return db.KhachThues
                     .Include(k => k.HopDongs)  // nếu cần load hợp đồng
                     .AsNoTracking()
                     .FirstOrDefault(k => k.MaKhach == maKhach);
        }

    }
}
