
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class DangKyHocPhanRepository : IDangKyHocPhanRepository
    {
        private readonly QlyDaoTaoContext _context;

        public DangKyHocPhanRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public Task<bool> SinhVienExistsAsync(string maSV)
        {
            return _context.SinhViens.AnyAsync(x =>
                x.MaSV == maSV && x.TrangThai);
        }

        public Task<bool> LopHocPhanExistsAsync(string maLopHocPhan)
        {
            return _context.LopHocPhans.AnyAsync(x =>
                x.MaLopHocPhan == maLopHocPhan &&
                x.TrangThai);
        }

        public Task<bool> DaDangKyAsync(
            string maSV, string maLopHocPhan)
        {
            return _context.DangKyHocPhans.AnyAsync(x =>
                x.MaSV == maSV &&
                x.MaLopHocPhan == maLopHocPhan &&
                x.TrangThai == "Đã đăng ký");
        }

        public Task<int> DemSoLuongDangKyAsync(string maLopHocPhan)
        {
            return _context.DangKyHocPhans.CountAsync(x =>
                x.MaLopHocPhan == maLopHocPhan &&
                x.TrangThai == "Đã đăng ký");
        }

        public Task<int?> LaySiSoToiDaAsync(string maLopHocPhan)
        {
            return _context.LopHocPhans
                .Where(x => x.MaLopHocPhan == maLopHocPhan)
                .Select(x => (int?)x.SiSoToiDa)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(DangKyHocPhan dangKy)
        {
            await _context.DangKyHocPhans.AddAsync(dangKy);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<DangKyHocPhan?> GetDangKyAsync(
            string maSV, string maLopHocPhan)
        {
            return _context.DangKyHocPhans.FirstOrDefaultAsync(x =>
                x.MaSV == maSV &&
                x.MaLopHocPhan == maLopHocPhan);
        }

        public Task UpdateAsync(DangKyHocPhan dangKy)
        {
            _context.DangKyHocPhans.Update(dangKy);
            return Task.CompletedTask;
        }

    }
}
