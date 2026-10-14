using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class GiangVienRepository : IGiangVienRepository
    {
        private readonly QlyDaoTaoContext _context;

        public GiangVienRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<GiangVien>> GetAllAsync()
        {
            return await _context.GiangViens
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GiangVien?> GetByIdAsync(string maGiangVien)
        {
            return await _context.GiangViens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaGiangVien == maGiangVien);
        }

        public async Task<List<GiangVien>> SearchAsync(string keyword)
        {
            return await _context.GiangViens
                .AsNoTracking()
                .Where(x =>
                    x.MaGiangVien.Contains(keyword) ||
                    x.HoTen.Contains(keyword) ||
                    (x.Email != null && x.Email.Contains(keyword)) ||
                    (x.SDT != null && x.SDT.Contains(keyword)))
                .ToListAsync();
        }

        public async Task AddAsync(GiangVien giangVien)
        {
            await _context.GiangViens.AddAsync(giangVien);
        }

        public async Task UpdateAsync(GiangVien giangVien)
        {
            _context.GiangViens.Update(giangVien);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(string maGiangVien)
        {
            var giangVien = await _context.GiangViens
                .FirstOrDefaultAsync(x => x.MaGiangVien == maGiangVien);

            if (giangVien != null)
            {
                _context.GiangViens.Remove(giangVien);
            }
        }

        public async Task<bool> ExistsAsync(string maGiangVien)
        {
            return await _context.GiangViens
                .AnyAsync(x => x.MaGiangVien == maGiangVien);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}