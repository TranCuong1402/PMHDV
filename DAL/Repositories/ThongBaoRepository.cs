using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class ThongBaoRepository : IThongBaoRepository
    {
        private readonly QlyDaoTaoContext _context;

        public ThongBaoRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<bool> LichHocExistsAsync(int maLichHoc)
        {
            return await _context.LichHocs
                .AnyAsync(x => x.MaLichHoc == maLichHoc);
        }

        public async Task<List<int>> GetTaiKhoanSinhVienAsync(
            string maLopHocPhan)
        {
            return await _context.DangKyHocPhans
                .Where(x =>
                    x.MaLopHocPhan == maLopHocPhan &&
                    x.TrangThai == "Đã đăng ký")
                .Join(
                    _context.SinhViens,
                    dk => dk.MaSV,
                    sv => sv.MaSV,
                    (dk, sv) => sv.IdTaiKhoan)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToListAsync();
        }

        public async Task<int?> GetTaiKhoanGiangVienAsync(
            int maLichHoc)
        {
            return await _context.LichHocs
                .Where(lh => lh.MaLichHoc == maLichHoc)
                .Join(
                    _context.LopHocPhans,
                    lh => lh.MaLopHocPhan,
                    lhp => lhp.MaLopHocPhan,
                    (lh, lhp) => lhp.MaGiangVien)
                .Join(
                    _context.GiangViens,
                    maGV => maGV,
                    gv => gv.MaGiangVien,
                    (maGV, gv) => gv.IdTaiKhoan)
                .FirstOrDefaultAsync();
        }

        public async Task AddRangeAsync(List<ThongBao> thongBaos)
        {
            await _context.ThongBaos.AddRangeAsync(thongBaos);
        }
        public async Task<string> GetMaLopHocPhanAsync(int maLichHoc)
        {
            var maLopHocPhan = await _context.LichHocs
                .Where(x => x.MaLichHoc == maLichHoc)
                .Select(x => x.MaLopHocPhan)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(maLopHocPhan))
                throw new KeyNotFoundException("Không tìm thấy lớp học phần của lịch học.");

            return maLopHocPhan;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}