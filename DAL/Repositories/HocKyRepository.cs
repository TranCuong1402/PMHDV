
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class HocKyRepository : IHocKyRepository
    {
        private readonly QlyDaoTaoContext _context;

        public HocKyRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<HocKy>> GetAllAsync()
        {
            return await _context.HocKys
                .AsNoTracking()
                .OrderByDescending(h => h.NgayBatDau)
                .ToListAsync();
        }

        public async Task<HocKy?> GetByIdAsync(string maHocKy)
        {
            return await _context.HocKys
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.MaHocKy == maHocKy);
        }

        public async Task<List<HocKy>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            return await _context.HocKys
                .AsNoTracking()
                .Where(h =>
                    h.MaHocKy.Contains(keyword) ||
                    h.TenHocKy.Contains(keyword) ||
                    h.MaNienKhoa.Contains(keyword))
                .OrderByDescending(h => h.NgayBatDau)
                .ToListAsync();
        }

        public async Task AddAsync(HocKy hocKy)
        {
            await _context.HocKys.AddAsync(hocKy);
        }

        public Task UpdateAsync(HocKy hocKy)
        {
            _context.HocKys.Update(hocKy);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(string maHocKy)
        {
            var hocKy = await _context.HocKys
                .FirstOrDefaultAsync(h => h.MaHocKy == maHocKy);

            if (hocKy != null)
                _context.HocKys.Remove(hocKy);
        }

        public async Task<bool> ExistsAsync(string maHocKy)
        {
            return await _context.HocKys
                .AnyAsync(h => h.MaHocKy == maHocKy);
        }

        public async Task<bool> ExistsNienKhoaAsync(string maNienKhoa)
        {
            return await _context.NienKhoas
                .AnyAsync(n => n.MaNienKhoa == maNienKhoa);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
