
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories
{
    public class LopHocPhanSinhVienRepository
        : ILopHocPhanSinhVienRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LopHocPhanSinhVienRepository(
            QlyDaoTaoContext context)
        {
            _context = context;
        }

        public Task<bool> SinhVienExistsAsync(string maSV)
        {
            return _context.SinhViens
                .AsNoTracking()
                .AnyAsync(x => x.MaSV == maSV);
        }

        public async Task<List<LopHocPhanSinhVienDto>>
            GetBySinhVienAsync(string maSV)
        {
            var query =
                from dk in _context.DangKyHocPhans.AsNoTracking()
                join lhp in _context.LopHocPhans.AsNoTracking()
                    on dk.MaLopHocPhan equals lhp.MaLopHocPhan
                join mh in _context.MonHocs.AsNoTracking()
                    on lhp.MaMonHoc equals mh.MaMonHoc
                join hk in _context.HocKys.AsNoTracking()
                    on lhp.MaHocKy equals hk.MaHocKy
                join gv in _context.GiangViens.AsNoTracking()
                    on lhp.MaGiangVien equals gv.MaGiangVien
                where dk.MaSV == maSV
                orderby dk.NgayDangKy descending
                select new LopHocPhanSinhVienDto
                {
                    MaSV = dk.MaSV,
                    MaLopHocPhan = lhp.MaLopHocPhan,
                    MaMonHoc = mh.MaMonHoc,
                    TenMonHoc = mh.TenMonHoc,
                    MaHocKy = hk.MaHocKy,
                    TenHocKy = hk.TenHocKy,
                    MaGiangVien = gv.MaGiangVien,
                    TenGiangVien = gv.HoTen,
                    SiSoToiDa = lhp.SiSoToiDa,
                    NgayDangKy = dk.NgayDangKy,
                    TrangThaiDangKy = dk.TrangThai
                };

            return await query.ToListAsync();
        }
    }
}
