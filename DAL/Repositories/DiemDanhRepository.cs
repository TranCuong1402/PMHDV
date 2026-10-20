
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class DiemDanhRepository : IDiemDanhRepository
    {
        private readonly QlyDaoTaoContext _context;

        public DiemDanhRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public Task<bool> LichHocExistsAsync(int maLichHoc)
        {
            return _context.LichHocs.AnyAsync(
                x => x.MaLichHoc == maLichHoc);
        }

        public Task<bool> SinhVienExistsAsync(string maSV)
        {
            return _context.SinhViens.AnyAsync(
                x => x.MaSV == maSV && x.TrangThai);
        }

        public Task<DiemDanh?> GetByLichHocAndSinhVienAsync(
            int maLichHoc, string maSV)
        {
            return _context.DiemDanhs.FirstOrDefaultAsync(x =>
                x.MaLichHoc == maLichHoc && x.MaSV == maSV);
        }

        public async Task AddAsync(DiemDanh diemDanh)
        {
            await _context.DiemDanhs.AddAsync(diemDanh);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public Task UpdateAsync(DiemDanh diemDanh)
        {
            _context.DiemDanhs.Update(diemDanh);
            return Task.CompletedTask;
        }

    }
}
