
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class NienKhoaRepository : INienKhoaRepository
    {
        private readonly QlyDaoTaoContext _context;

        public NienKhoaRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<NienKhoa>> GetAllAsync()
        {
            return await _context.NienKhoas
                .AsNoTracking()
                .OrderByDescending(n => n.NamBatDau)
                .ToListAsync();
        }

        public async Task<NienKhoa?> GetByIdAsync(string maNienKhoa)
        {
            return await _context.NienKhoas
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.MaNienKhoa == maNienKhoa);
        }

        public async Task<List<NienKhoa>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            return await _context.NienKhoas
                .AsNoTracking()
                .Where(n =>
                    n.MaNienKhoa.Contains(keyword) ||
                    n.TenNienKhoa.Contains(keyword))
                .OrderByDescending(n => n.NamBatDau)
                .ToListAsync();
        }

        public async Task AddAsync(NienKhoa nienKhoa)
        {
            await _context.NienKhoas.AddAsync(nienKhoa);
        }

        public Task UpdateAsync(NienKhoa nienKhoa)
        {
            _context.NienKhoas.Update(nienKhoa);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(string maNienKhoa)
        {
            var entity = await _context.NienKhoas
                .FirstOrDefaultAsync(n => n.MaNienKhoa == maNienKhoa);

            if (entity != null)
            {
                _context.NienKhoas.Remove(entity);
            }
        }

        public async Task<bool> ExistsAsync(string maNienKhoa)
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
