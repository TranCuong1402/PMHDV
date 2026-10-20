
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories
{
    public class TyLeVangRepository : ITyLeVangRepository
    {
        private readonly QlyDaoTaoContext _context;

        public TyLeVangRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<bool> SinhVienExistsAsync(string maSV)
        {
            return await _context.SinhViens
                .AnyAsync(sv => sv.MaSV == maSV);
        }

        public async Task<List<TyLeVangDto>> TinhTyLeVangAsync(
            string maSV)
        {
            var ketQua = await (
                from dd in _context.DiemDanhs.AsNoTracking()
                join lh in _context.LichHocs.AsNoTracking()
                    on dd.MaLichHoc equals lh.MaLichHoc
                where dd.MaSV == maSV
                group dd by lh.MaLopHocPhan into nhom
                select new
                {
                    MaLopHocPhan = nhom.Key,
                    TongSoBuoi = nhom.Count(),
                    SoBuoiVang = nhom.Count(x => x.TrangThai == "Vắng"),
                    SoBuoiCoPhep = nhom.Count(x => x.TrangThai == "Có phép"),
                    SoBuoiCoMat = nhom.Count(x => x.TrangThai == "Có mặt"),
                    SoBuoiMuon = nhom.Count(x => x.TrangThai == "Muộn")
                }
            ).ToListAsync();

            return ketQua.Select(x => new TyLeVangDto
            {
                MaSV = maSV,
                MaLopHocPhan = x.MaLopHocPhan,
                TongSoBuoi = x.TongSoBuoi,
                SoBuoiVang = x.SoBuoiVang,
                SoBuoiCoPhep = x.SoBuoiCoPhep,
                SoBuoiCoMat = x.SoBuoiCoMat,
                SoBuoiMuon = x.SoBuoiMuon,
                TyLeVang = x.TongSoBuoi == 0
                    ? 0
                    : Math.Round(
                        (decimal)x.SoBuoiVang / x.TongSoBuoi * 100,
                        2)
            }).ToList();
        }
    }
}
