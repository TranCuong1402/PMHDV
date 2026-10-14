using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class LopRepository : ILopRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LopRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<Lop>> GetAllAsync()
        {
            return await _context.Lops
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Lop?> GetByIdAsync(string maLop)
        {
            return await _context.Lops
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaLop == maLop);
        }

        public async Task<List<Lop>> SearchAsync(string keyword)
        {
            return await _context.Lops
                .AsNoTracking()
                .Where(x =>
                    x.MaLop.Contains(keyword) ||
                    x.TenLop.Contains(keyword) ||
                    x.MaNienKhoa.Contains(keyword) ||
                    (x.Khoa != null && x.Khoa.Contains(keyword)))
                .ToListAsync();
        }

        public async Task AddAsync(Lop lop)
        {
            await _context.Lops.AddAsync(lop);
        }

        public async Task UpdateAsync(Lop lop)
        {
            _context.Lops.Update(lop);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string maLop)
        {
            var lop = await _context.Lops
                .FirstOrDefaultAsync(x => x.MaLop == maLop);

            if (lop != null)
            {
                _context.Lops.Remove(lop);
            }
        }

        public async Task<bool> ExistsAsync(string maLop)
        {
            return await _context.Lops
                .AnyAsync(x => x.MaLop == maLop);
        }

        public async Task<bool> ExistsNienKhoaAsync(string maNienKhoa)
        {
            return await _context.NienKhoas
                .AnyAsync(x => x.MaNienKhoa == maNienKhoa);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}