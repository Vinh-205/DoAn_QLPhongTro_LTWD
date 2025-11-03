using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Phong_Tro_DAL.PhongTro; // đúng namespace DAL

namespace Phong_Tro_BUS
{
    public class TienIchBUS : IDisposable
    {
        private readonly Connect db;

        public TienIchBUS()
        {
            db = new Connect();
        }

        // =================== LẤY TẤT CẢ ===================
        public List<TienIch> LayTatCa()
        {
            try
            {
                return db.TienIches.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tải danh sách tiện ích: " + ex.Message);
            }
        }

        // =================== THÊM ===================
        public bool Them(TienIch ti)
        {
            try
            {
                if (ti == null)
                    throw new ArgumentNullException(nameof(ti), "Dữ liệu tiện ích không hợp lệ!");

                bool tonTai = db.TienIches.Any(x => x.TenTienIch.Trim().ToLower() == ti.TenTienIch.Trim().ToLower());
                if (tonTai)
                    throw new Exception("Tên tiện ích đã tồn tại!");

                db.TienIches.Add(ti);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm tiện ích: " + ex.Message);
            }
        }

        // =================== SỬA ===================
        public bool Sua(TienIch ti)
        {
            try
            {
                if (ti == null)
                    throw new ArgumentNullException(nameof(ti));

                var existing = db.TienIches.Find(ti.MaTienIch);
                if (existing == null)
                    throw new Exception("Không tìm thấy tiện ích để cập nhật!");

                bool trungTen = db.TienIches.Any(x =>
                    x.TenTienIch.Trim().ToLower() == ti.TenTienIch.Trim().ToLower() &&
                    x.MaTienIch != ti.MaTienIch);

                if (trungTen)
                    throw new Exception("Tên tiện ích đã tồn tại, vui lòng chọn tên khác!");

                existing.TenTienIch = ti.TenTienIch;
                existing.DonGia = ti.DonGia;
                existing.MoTa = ti.MoTa;

                db.Entry(existing).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật tiện ích: " + ex.Message);
            }
        }

        // =================== XÓA ===================
        public bool Xoa(int maTienIch)
        {
            try
            {
                var ti = db.TienIches
                    .Include(x => x.ChiTietTienIches)
                    .FirstOrDefault(x => x.MaTienIch == maTienIch);

                if (ti == null)
                    throw new Exception("Không tìm thấy tiện ích cần xóa!");

                if (ti.ChiTietTienIches != null && ti.ChiTietTienIches.Any())
                    throw new Exception("Không thể xóa! Tiện ích đang được sử dụng trong chi tiết phòng.");

                db.TienIches.Remove(ti);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa tiện ích: " + ex.Message);
            }
        }

        // =================== TÌM KIẾM ===================
        public List<TienIch> TimKiem(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                    return LayTatCa();

                tuKhoa = tuKhoa.Trim().ToLower();

                return db.TienIches.AsNoTracking()
                    .Where(x =>
                        x.TenTienIch.ToLower().Contains(tuKhoa) ||
                        (x.MoTa != null && x.MoTa.ToLower().Contains(tuKhoa)))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm tiện ích: " + ex.Message);
            }
        }

        // =================== LẤY THEO MÃ ===================
        public TienIch LayTheoMa(int maTienIch)
        {
            try
            {
                return db.TienIches.AsNoTracking()
                                   .FirstOrDefault(x => x.MaTienIch == maTienIch);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tiện ích theo mã: " + ex.Message);
            }
        }

        // =================== HỦY (DISPOSE) ===================
        public void Dispose()
        {
            db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
