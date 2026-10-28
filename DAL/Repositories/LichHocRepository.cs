
using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class LichHocRepository : ILichHocRepository
    {
        private readonly QlyDaoTaoContext _context;

        public LichHocRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<List<LichHoc>> GetAllAsync()
        {
            return await _context.LichHocs
                .AsNoTracking()
                .OrderBy(l => l.NgayHoc)
                .ThenBy(l => l.GioBatDau)
                .ToListAsync();
        }

        public async Task<LichHoc?> GetByIdAsync(int maLichHoc)
        {
            return await _context.LichHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.MaLichHoc == maLichHoc);
        }

        public async Task<List<LichHoc>> SearchAsync(string keyword)
        {
            keyword = keyword.Trim();

            return await _context.LichHocs
                .AsNoTracking()
                .Where(l =>
                    l.MaLopHocPhan.Contains(keyword) ||
                    (l.MaPhong != null && l.MaPhong.Contains(keyword)) ||
                    l.TrangThai.Contains(keyword))
                .OrderBy(l => l.NgayHoc)
                .ThenBy(l => l.GioBatDau)
                .ToListAsync();
        }

        public async Task AddAsync(LichHoc lichHoc)
        {
            await _context.LichHocs.AddAsync(lichHoc);
        }

        public Task UpdateAsync(LichHoc lichHoc)
        {
            _context.LichHocs.Update(lichHoc);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int maLichHoc)
        {
            var entity = await _context.LichHocs
                .FirstOrDefaultAsync(l => l.MaLichHoc == maLichHoc);

            if (entity != null)
                _context.LichHocs.Remove(entity);
        }

        public async Task<bool> ExistsAsync(int maLichHoc)
        {
            return await _context.LichHocs
                .AnyAsync(l => l.MaLichHoc == maLichHoc);
        }

        public async Task<bool> ExistsLopHocPhanAsync(string maLopHocPhan)
        {
            return await _context.LopHocPhans
                .AnyAsync(l => l.MaLopHocPhan == maLopHocPhan);
        }

        public async Task<bool> ExistsPhongAsync(string maPhong)
        {
            return await _context.Phongs
                .AnyAsync(p => p.MaPhong == maPhong);
        }

        public async Task<bool> HasScheduleConflictAsync(
            string maLopHocPhan,
            string? maPhong,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc,
            int? excludeMaLichHoc = null)
        {
            var query = _context.LichHocs
                .Where(l =>
                    l.NgayHoc.Date == ngayHoc.Date &&
                    l.GioBatDau < gioKetThuc &&
                    l.GioKetThuc > gioBatDau);

            if (excludeMaLichHoc.HasValue)
            {
                query = query.Where(
                    l => l.MaLichHoc != excludeMaLichHoc.Value);
            }

            // Một lớp học phần không thể có hai lịch trùng giờ.
            bool conflictLop = await query.AnyAsync(
                l => l.MaLopHocPhan == maLopHocPhan);

            if (conflictLop)
                return true;

            // Một phòng không thể được sử dụng cho hai lịch cùng giờ.
            if (!string.IsNullOrWhiteSpace(maPhong))
            {
                bool conflictPhong = await query.AnyAsync(
                    l => l.MaPhong == maPhong);

                if (conflictPhong)
                    return true;
            }

            return false;
        }

        public async Task<string?> GetMaGiangVienByLichHocAsync(
            int maLichHoc)
        {
            return await (
                from lh in _context.LichHocs
                join lhp in _context.LopHocPhans
                    on lh.MaLopHocPhan equals lhp.MaLopHocPhan
                where lh.MaLichHoc == maLichHoc
                select lhp.MaGiangVien
            ).FirstOrDefaultAsync();
        }

        public async Task<bool> HasScheduleConflictForUpdateAsync(
            int maLichHoc,
            string maLopHocPhan,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc)
        {
            var lichHienTai = await _context.LichHocs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaLichHoc == maLichHoc);

            if (lichHienTai == null)
                return false;

            // Kiểm tra trùng lịch của cùng lớp học phần.
            bool trungLop = await _context.LichHocs
                .AnyAsync(x =>
                    x.MaLichHoc != maLichHoc &&
                    x.MaLopHocPhan == maLopHocPhan &&
                    x.NgayHoc == ngayHoc.Date &&
                    x.GioBatDau < gioKetThuc &&
                    x.GioKetThuc > gioBatDau);

            if (trungLop)
                return true;

            // Kiểm tra trùng lịch giảng viên ở lớp học phần khác.
            var maGiangVien = await _context.LopHocPhans
                .Where(x => x.MaLopHocPhan == maLopHocPhan)
                .Select(x => x.MaGiangVien)
                .FirstOrDefaultAsync();

            if (maGiangVien == null)
                return false;

            return await (
                from lh in _context.LichHocs
                join lhp in _context.LopHocPhans
                    on lh.MaLopHocPhan equals lhp.MaLopHocPhan
                where lh.MaLichHoc != maLichHoc
                      && lhp.MaGiangVien == maGiangVien
                      && lh.NgayHoc == ngayHoc.Date
                      && lh.GioBatDau < gioKetThuc
                      && lh.GioKetThuc > gioBatDau
                select lh
            ).AnyAsync();
        }

        public async Task<bool> PhongExistsAsync(string maPhong)
        {
            return await _context.Phongs
                .AnyAsync(p => p.MaPhong == maPhong && p.TrangThai);
        }

        public async Task<bool> HasRoomConflictAsync(
            int maLichHoc,
            string maPhong,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc)
        {
            return await _context.LichHocs
                .AnyAsync(lh =>
                    lh.MaLichHoc != maLichHoc &&
                    lh.MaPhong == maPhong &&
                    lh.NgayHoc == ngayHoc.Date &&
                    lh.GioBatDau < gioKetThuc &&
                    lh.GioKetThuc > gioBatDau);
        }

        public async Task<KiemTraTrungLichResponse> KiemTraTrungLichAsync(
            KiemTraTrungLichRequest request)
        {
            var danhSachLoi = new List<string>();

            var lopHocPhan = await _context.LopHocPhans
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.MaLopHocPhan == request.MaLopHocPhan);

            if (lopHocPhan == null)
            {
                danhSachLoi.Add("Lớp học phần không tồn tại.");

                return new KiemTraTrungLichResponse
                {
                    HopLe = false,
                    DanhSachLoi = danhSachLoi,
                    ThongBao = "Dữ liệu lịch học không hợp lệ."
                };
            }

            int? maLichBoQua = request.MaLichHoc;

            var lichTrongNgay = _context.LichHocs
                .AsNoTracking()
                .Where(x =>
                    x.NgayHoc == request.NgayHoc.Date &&
                    (!maLichBoQua.HasValue ||
                     x.MaLichHoc != maLichBoQua.Value) &&
                    x.GioBatDau < request.GioKetThuc &&
                    x.GioKetThuc > request.GioBatDau);

            // 1. Trùng lịch của cùng lớp học phần.
            bool trungLop = await lichTrongNgay.AnyAsync(x =>
                x.MaLopHocPhan == request.MaLopHocPhan);

            if (trungLop)
            {
                danhSachLoi.Add(
                    "Lớp học phần đã có buổi học trùng thời gian.");
            }

            // 2. Trùng lịch giảng viên ở các lớp học phần khác.
            bool trungGiangVien = await (
                from lh in lichTrongNgay
                join lhp in _context.LopHocPhans.AsNoTracking()
                    on lh.MaLopHocPhan equals lhp.MaLopHocPhan
                where lhp.MaGiangVien == lopHocPhan.MaGiangVien
                select lh.MaLichHoc
            ).AnyAsync();

            if (trungGiangVien)
            {
                danhSachLoi.Add(
                    "Giảng viên đã có lịch dạy khác trong khung giờ này.");
            }

            // 3. Trùng phòng nếu yêu cầu có chọn phòng.
            if (!string.IsNullOrWhiteSpace(request.MaPhong))
            {
                bool phongTonTai = await _context.Phongs
                    .AnyAsync(p =>
                        p.MaPhong == request.MaPhong &&
                        p.TrangThai);

                if (!phongTonTai)
                {
                    danhSachLoi.Add(
                        "Phòng học không tồn tại hoặc đang không hoạt động.");
                }
                else
                {
                    bool trungPhong = await lichTrongNgay.AnyAsync(x =>
                        x.MaPhong == request.MaPhong);

                    if (trungPhong)
                    {
                        danhSachLoi.Add(
                            "Phòng học đã được sử dụng trong khung giờ này.");
                    }
                }
            }

            return new KiemTraTrungLichResponse
            {
                HopLe = danhSachLoi.Count == 0,
                DanhSachLoi = danhSachLoi,
                ThongBao = danhSachLoi.Count == 0
                    ? "Lịch học hợp lệ, không phát hiện xung đột."
                    : "Lịch học có xung đột."
            };
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
