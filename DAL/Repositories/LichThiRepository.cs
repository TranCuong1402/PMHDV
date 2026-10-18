
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class LichThiRepository : ILichThiRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LichThiRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<LichThi>> GetAllAsync()
        {
            return await _context.LichThis
                .AsNoTracking()
                .OrderBy(l => l.NgayThi)
                .ThenBy(l => l.GioBatDau)
                .ToListAsync();
        }

        public async Task<LichThi?> GetByIdAsync(int maLichThi)
        {
            return await _context.LichThis
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.MaLichThi == maLichThi);
        }

        public async Task<List<LichThi>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            return await _context.LichThis
                .AsNoTracking()
                .Where(l =>
                    l.MaLopHocPhan.Contains(keyword) ||
                    (l.MaPhong != null && l.MaPhong.Contains(keyword)) ||
                    (l.HinhThucThi != null &&
                     l.HinhThucThi.Contains(keyword)))
                .OrderBy(l => l.NgayThi)
                .ThenBy(l => l.GioBatDau)
                .ToListAsync();
        }

        public async Task AddAsync(LichThi lichThi)
        {
            await _context.LichThis.AddAsync(lichThi);
        }

        public Task UpdateAsync(LichThi lichThi)
        {
            _context.LichThis.Update(lichThi);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int maLichThi)
        {
            var entity = await _context.LichThis
                .FirstOrDefaultAsync(l => l.MaLichThi == maLichThi);

            if (entity != null)
                _context.LichThis.Remove(entity);
        }

        public async Task<bool> ExistsLopHocPhanAsync(
            string maLopHocPhan)
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
            DateTime ngayThi,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc,
            int? excludeMaLichThi = null)
        {
            var query = _context.LichThis
                .Where(l =>
                    l.NgayThi == ngayThi.Date &&
                    l.GioBatDau < gioKetThuc &&
                    l.GioKetThuc > gioBatDau);

            if (excludeMaLichThi.HasValue)
            {
                query = query.Where(
                    l => l.MaLichThi != excludeMaLichThi.Value);
            }

            // Một lớp học phần không thể có hai lịch thi trùng giờ.
            if (await query.AnyAsync(
                l => l.MaLopHocPhan == maLopHocPhan))
            {
                return true;
            }

            // Một phòng không thể tổ chức hai kỳ thi cùng thời điểm.
            if (!string.IsNullOrWhiteSpace(maPhong) &&
                await query.AnyAsync(l => l.MaPhong == maPhong))
            {
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
