
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories
{
    public class LichGiangDayRepository : ILichGiangDayRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LichGiangDayRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<bool> GiangVienExistsAsync(string maGiangVien)
        {
            return await _context.GiangViens
                .AnyAsync(gv => gv.MaGiangVien == maGiangVien);
        }

        public async Task<List<LichGiangDayDto>> GetByGiangVienAsync(
            string maGiangVien)
        {
            var query =
                from lh in _context.LichHocs.AsNoTracking()
                join lhp in _context.LopHocPhans.AsNoTracking()
                    on lh.MaLopHocPhan equals lhp.MaLopHocPhan
                join mh in _context.MonHocs.AsNoTracking()
                    on lhp.MaMonHoc equals mh.MaMonHoc
                where lhp.MaGiangVien == maGiangVien
                orderby lh.NgayHoc, lh.GioBatDau
                select new LichGiangDayDto
                {
                    MaLichHoc = lh.MaLichHoc,
                    MaGiangVien = lhp.MaGiangVien,
                    MaLopHocPhan = lhp.MaLopHocPhan,
                    MaMonHoc = mh.MaMonHoc,
                    TenMonHoc = mh.TenMonHoc,
                    NgayHoc = lh.NgayHoc,
                    GioBatDau = lh.GioBatDau,
                    GioKetThuc = lh.GioKetThuc,
                    MaPhong = lh.MaPhong,
                    TrangThai = lh.TrangThai
                };

            return await query.ToListAsync();
        }
    }
}
