using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class SinhVienRepository : ISinhVienRepository
    {
        private readonly QlyDaoTaoContext _context;

        public SinhVienRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        // ============================
        // LẤY TẤT CẢ SINH VIÊN
        // ============================
        public async Task<List<SinhVien>> GetAllAsync()
        {
            return await _context.SinhViens
                .AsNoTracking()
                .ToListAsync();
        }

        // ============================
        // LẤY SINH VIÊN THEO MÃ
        // ============================
        public async Task<SinhVien?> GetByIdAsync(string maSV)
        {
            return await _context.SinhViens
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaSV == maSV);
        }

        // ============================
        // TÌM KIẾM SINH VIÊN
        // ============================
        public async Task<List<SinhVien>> SearchAsync(string keyword)
        {
            return await _context.SinhViens
                .AsNoTracking()
                .Where(x =>
                    x.MaSV.Contains(keyword) ||
                    x.HoTen.Contains(keyword) ||
                    (x.Email != null && x.Email.Contains(keyword)) ||
                    (x.SDT != null && x.SDT.Contains(keyword)))
                .ToListAsync();
        }

        // ============================
        // THÊM SINH VIÊN
        // ============================
        public async Task AddAsync(SinhVien sinhVien)
        {
            await _context.SinhViens.AddAsync(sinhVien);
        }

        // ============================
        // CẬP NHẬT SINH VIÊN
        // ============================
        public async Task UpdateAsync(SinhVien sinhVien)
        {
            _context.SinhViens.Update(sinhVien);

            await Task.CompletedTask;
        }

        // ============================
        // XÓA SINH VIÊN
        // ============================
        public async Task DeleteAsync(string maSV)
        {
            var sinhVien = await _context.SinhViens
                .FirstOrDefaultAsync(x => x.MaSV == maSV);

            if (sinhVien != null)
            {
                _context.SinhViens.Remove(sinhVien);
            }
        }

        // ============================
        // KIỂM TRA SINH VIÊN TỒN TẠI
        // ============================
        public async Task<bool> ExistsAsync(string maSV)
        {
            return await _context.SinhViens
                .AnyAsync(x => x.MaSV == maSV);
        }

        // ============================
        // LƯU DATABASE
        // ============================
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}