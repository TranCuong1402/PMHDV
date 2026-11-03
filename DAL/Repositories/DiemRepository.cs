using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.Model.DTOs;
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

        public async Task<Diem?> GetBySinhVienAndLopHocPhanAsync(
            string maSV,
            string maLopHocPhan)
        {
            return await _context.Diems.FirstOrDefaultAsync(d =>
                d.MaSV == maSV &&
                d.MaLopHocPhan == maLopHocPhan);
        }

        public async Task<List<XemDiemDto>> GetDiemBySinhVienAsync(
            string maSV)
        {
            return await (
                from d in _context.Diems
                join lhp in _context.LopHocPhans
                    on d.MaLopHocPhan equals lhp.MaLopHocPhan
                join mh in _context.MonHocs
                    on lhp.MaMonHoc equals mh.MaMonHoc
                select new XemDiemDto
                {
                    MaSV = d.MaSV,
                    MaLopHocPhan = d.MaLopHocPhan,
                    MaMonHoc = mh.MaMonHoc,
                    TenMonHoc = mh.TenMonHoc,
                    MaHocKy = lhp.MaHocKy,
                    DiemChuyenCan = d.DiemChuyenCan,
                    DiemGiuaKy = d.DiemGiuaKy,
                    DiemCuoiKy = d.DiemCuoiKy,
                    DiemTongKet = d.DiemTongKet,
                    XepLoai = d.XepLoai,
                    UpdatedAt = d.UpdatedAt
                })
                .Where(d => d.MaSV == maSV)
                .OrderBy(d => d.MaMonHoc)
                .ToListAsync();
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