
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories
{
    public class LichSuDiemDanhRepository
        : ILichSuDiemDanhRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LichSuDiemDanhRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<bool> SinhVienExistsAsync(string maSV)
        {
            return await _context.SinhViens
                .AnyAsync(sv => sv.MaSV == maSV);
        }

        public async Task<List<LichSuDiemDanhDto>> GetBySinhVienAsync(
            string maSV)
        {
            var query =
                from dd in _context.DiemDanhs.AsNoTracking()
                join lh in _context.LichHocs.AsNoTracking()
                    on dd.MaLichHoc equals lh.MaLichHoc
                where dd.MaSV == maSV
                orderby lh.NgayHoc descending,
                        lh.GioBatDau descending
                select new LichSuDiemDanhDto
                {
                    Id = dd.Id,
                    MaLichHoc = lh.MaLichHoc,
                    MaSV = dd.MaSV,
                    MaLopHocPhan = lh.MaLopHocPhan,
                    NgayHoc = lh.NgayHoc,
                    GioBatDau = lh.GioBatDau,
                    GioKetThuc = lh.GioKetThuc,
                    MaPhong = lh.MaPhong,
                    TrangThai = dd.TrangThai,
                    ThoiGianDiemDanh = dd.ThoiGianDiemDanh,
                    LyDo = dd.LyDo,
                    NguoiDiemDanh = dd.NguoiDiemDanh
                };

            return await query.ToListAsync();
        }
    }
}
