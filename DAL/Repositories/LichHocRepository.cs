
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class LichHocRepository : ILichHocRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LichHocRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<LichHoc>> GetAllAsync()
        {
            return await _context.LichHocs
                .AsNoTracking()
                .OrderBy(l => l.NgayHoc)
                .ThenBy(l => l.GioBatDau)
                .ToListAsync();
        }

        public async Task<LichHoc?> GetByIdAsync(int maLichHoc)
        {
            return await _context.LichHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.MaLichHoc == maLichHoc);
        }

        public async Task<List<LichHoc>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            return await _context.LichHocs
                .AsNoTracking()
                .Where(l =>
                    l.MaLopHocPhan.Contains(keyword) ||
                    (l.MaPhong != null && l.MaPhong.Contains(keyword)) ||
                    l.TrangThai.Contains(keyword))
                .OrderBy(l => l.NgayHoc)
                .ThenBy(l => l.GioBatDau)
                .ToListAsync();
        }

        public async Task AddAsync(LichHoc lichHoc)
        {
            await _context.LichHocs.AddAsync(lichHoc);
        }

        public Task UpdateAsync(LichHoc lichHoc)
        {
            _context.LichHocs.Update(lichHoc);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int maLichHoc)
        {
            var entity = await _context.LichHocs
                .FirstOrDefaultAsync(l => l.MaLichHoc == maLichHoc);

            if (entity != null)
                _context.LichHocs.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int maLichHoc)
        {
            return await _context.LichHocs
                .AnyAsync(l => l.MaLichHoc == maLichHoc);
        }

        public async Task<bool> ExistsLopHocPhanAsync(string maLopHocPhan)
        {
            return await _context.LopHocPhans
                .AnyAsync(l => l.MaLopHocPhan == maLopHocPhan);
        }

        public async Task<bool> ExistsPhongAsync(string maPhong)
        {
            return await _context.Phongs
                .AnyAsync(p => p.MaPhong == maPhong);
        }

        public async Task<bool> HasScheduleConflictAsync(
            string maLopHocPhan,
            string? maPhong,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc,
            int? excludeMaLichHoc = null)
        {
            var query = _context.LichHocs
                .Where(l =>
                    l.NgayHoc.Date == ngayHoc.Date &&
                    l.GioBatDau < gioKetThuc &&
                    l.GioKetThuc > gioBatDau);

            if (excludeMaLichHoc.HasValue)
            {
                query = query.Where(
                    l => l.MaLichHoc != excludeMaLichHoc.Value);
            }

            // Một lớp học phần không thể có hai lịch trùng giờ.
            bool conflictLop = await query.AnyAsync(
                l => l.MaLopHocPhan == maLopHocPhan);

            if (conflictLop)
                return true;

            // Một phòng không thể được sử dụng cho hai lịch cùng giờ.
            if (!string.IsNullOrWhiteSpace(maPhong))
            {
                bool conflictPhong = await query.AnyAsync(
                    l => l.MaPhong == maPhong);

                if (conflictPhong)
                    return true;
            }

            return false;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
