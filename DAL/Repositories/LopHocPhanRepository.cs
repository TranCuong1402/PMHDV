using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories
{
    public class LopHocPhanRepository : ILopHocPhanRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LopHocPhanRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<LopHocPhanDto>> GetAllAsync()
        {
            return await BuildQuery().ToListAsync();
        }

        public async Task<LopHocPhanDto?> GetByIdAsync(string maLopHocPhan)
        {
            return await BuildQuery()
                .FirstOrDefaultAsync(x =>
                    x.MaLopHocPhan == maLopHocPhan);
        }

        public async Task<List<LopHocPhanDto>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            return await BuildQuery()
                .Where(x =>
                    x.MaLopHocPhan.Contains(keyword) ||
                    x.MaMonHoc.Contains(keyword) ||
                    x.TenMonHoc.Contains(keyword) ||
                    x.MaHocKy.Contains(keyword) ||
                    x.TenHocKy.Contains(keyword) ||
                    x.MaGiangVien.Contains(keyword) ||
                    x.TenGiangVien.Contains(keyword))
                .ToListAsync();
        }

        private IQueryable<LopHocPhanDto> BuildQuery()
        {
            return
                from lhp in _context.LopHocPhans.AsNoTracking()
                join mh in _context.MonHocs.AsNoTracking()
                    on lhp.MaMonHoc equals mh.MaMonHoc
                join hk in _context.HocKys.AsNoTracking()
                    on lhp.MaHocKy equals hk.MaHocKy
                join gv in _context.GiangViens.AsNoTracking()
                    on lhp.MaGiangVien equals gv.MaGiangVien
                orderby lhp.MaLopHocPhan
                select new LopHocPhanDto
                {
                    MaLopHocPhan = lhp.MaLopHocPhan,
                    MaMonHoc = mh.MaMonHoc,
                    TenMonHoc = mh.TenMonHoc,
                    MaHocKy = hk.MaHocKy,
                    TenHocKy = hk.TenHocKy,
                    MaGiangVien = gv.MaGiangVien,
                    TenGiangVien = gv.HoTen,
                    SiSoToiDa = lhp.SiSoToiDa,
                    TrangThai = lhp.TrangThai
                };
        }
    }
}