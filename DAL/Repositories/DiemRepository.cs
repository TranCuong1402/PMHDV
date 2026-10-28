using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class DiemRepository : IDiemRepository
    {
        private readonly QlyDaoTaoContext _context;

        public DiemRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public Task<bool> SinhVienExistsAsync(string maSV)
        {
            return _context.SinhViens
                .AnyAsync(x => x.MaSV == maSV);
        }

        public Task<bool> LopHocPhanExistsAsync(string maLopHocPhan)
        {
            return _context.LopHocPhans
                .AnyAsync(x => x.MaLopHocPhan == maLopHocPhan);
        }

        public Task<bool> SinhVienDaDangKyAsync(
            string maSV, string maLopHocPhan)
        {
            return _context.DangKyHocPhans.AnyAsync(x =>
                x.MaSV == maSV &&
                x.MaLopHocPhan == maLopHocPhan &&
                x.TrangThai == "Đã đăng ký");
        }

        public Task<bool> DiemExistsAsync(
            string maSV, string maLopHocPhan)
        {
            return _context.Diems.AnyAsync(x =>
                x.MaSV == maSV &&
                x.MaLopHocPhan == maLopHocPhan);
        }

        public async Task AddAsync(Diem diem)
        {
            await _context.Diems.AddAsync(diem);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}