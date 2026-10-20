
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class CanhBaoVangRepository : ICanhBaoVangRepository
    {
        private readonly QlyDaoTaoContext _context;

        public CanhBaoVangRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<bool> SinhVienExistsAsync(string maSV)
        {
            return await _context.SinhViens
                .AnyAsync(x => x.MaSV == maSV);
        }

        public async Task<bool> LopHocPhanExistsAsync(
            string maLopHocPhan)
        {
            return await _context.LopHocPhans
                .AnyAsync(x => x.MaLopHocPhan == maLopHocPhan);
        }

        public async Task<CanhBaoVang?> GetBySinhVienAndLopAsync(
            string maSV,
            string maLopHocPhan)
        {
            return await _context.CanhBaoVangs
                .FirstOrDefaultAsync(x =>
                    x.MaSV == maSV &&
                    x.MaLopHocPhan == maLopHocPhan);
        }

        public async Task AddAsync(CanhBaoVang canhBao)
        {
            await _context.CanhBaoVangs.AddAsync(canhBao);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<CanhBaoVang>> GetBySinhVienAsync(string maSV)
        {
            return await _context.CanhBaoVangs
                .AsNoTracking()
                .Where(x => x.MaSV == maSV)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
    }
}
