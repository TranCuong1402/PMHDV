using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class MonHocRepository : IMonHocRepository
    {
        private readonly QlyDaoTaoContext _context;

        public MonHocRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<MonHoc>> GetAllAsync()
        {
            return await _context.MonHocs
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<MonHoc?> GetByIdAsync(string maMonHoc)
        {
            return await _context.MonHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaMonHoc == maMonHoc);
        }

        public async Task<List<MonHoc>> SearchAsync(string keyword)
        {
            return await _context.MonHocs
                .AsNoTracking()
                .Where(x =>
                    x.MaMonHoc.Contains(keyword) ||
                    x.TenMonHoc.Contains(keyword) ||
                    (x.MoTa != null && x.MoTa.Contains(keyword)))
                .ToListAsync();
        }

        public async Task AddAsync(MonHoc monHoc)
        {
            await _context.MonHocs.AddAsync(monHoc);
        }

        public async Task UpdateAsync(MonHoc monHoc)
        {
            _context.MonHocs.Update(monHoc);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string maMonHoc)
        {
            var monHoc = await _context.MonHocs
                .FirstOrDefaultAsync(x => x.MaMonHoc == maMonHoc);

            if (monHoc != null)
            {
                _context.MonHocs.Remove(monHoc);
            }
        }

        public async Task<bool> ExistsAsync(string maMonHoc)
        {
            return await _context.MonHocs
                .AnyAsync(x => x.MaMonHoc == maMonHoc);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}