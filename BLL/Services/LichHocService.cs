
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class LichHocService : ILichHocService
    {
        private readonly ILichHocRepository _repository;

        public LichHocService(ILichHocRepository repository)
        {
            _repository = repository;
        }

        public Task<List<LichHoc>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<LichHoc?> GetByIdAsync(int maLichHoc)
            => _repository.GetByIdAsync(maLichHoc);

        public Task<List<LichHoc>> SearchAsync(string keyword)
            => _repository.SearchAsync(keyword);

        public async Task<(bool Success, string Message)> AddAsync(
            LichHoc lichHoc)
        {
            if (lichHoc == null)
                return (false, "Dữ liệu lịch học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(lichHoc.MaLopHocPhan))
                return (false, "Mã lớp học phần không được để trống.");

            if (lichHoc.GioKetThuc <= lichHoc.GioBatDau)
                return (false, "Giờ kết thúc phải sau giờ bắt đầu.");

            if (!await _repository.ExistsLopHocPhanAsync(
                lichHoc.MaLopHocPhan.Trim()))
                return (false, "Lớp học phần không tồn tại.");

            if (!string.IsNullOrWhiteSpace(lichHoc.MaPhong) &&
                !await _repository.ExistsPhongAsync(lichHoc.MaPhong.Trim()))
                return (false, "Phòng học không tồn tại.");

            if (await _repository.HasScheduleConflictAsync(
                lichHoc.MaLopHocPhan,
                lichHoc.MaPhong,
                lichHoc.NgayHoc,
                lichHoc.GioBatDau,
                lichHoc.GioKetThuc))
                return (false, "Lịch học bị trùng giờ với lịch khác.");

            lichHoc.MaLopHocPhan = lichHoc.MaLopHocPhan.Trim();
            lichHoc.MaPhong = string.IsNullOrWhiteSpace(lichHoc.MaPhong)
                ? null
                : lichHoc.MaPhong.Trim();

            await _repository.AddAsync(lichHoc);
            await _repository.SaveChangesAsync();

            return (true, "Thêm lịch học thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            int maLichHoc, LichHoc lichHoc)
        {
            if (lichHoc == null)
                return (false, "Dữ liệu lịch học không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maLichHoc);

            if (existing == null)
                return (false, "Không tìm thấy lịch học.");

            if (string.IsNullOrWhiteSpace(lichHoc.MaLopHocPhan))
                return (false, "Mã lớp học phần không được để trống.");

            if (lichHoc.GioKetThuc <= lichHoc.GioBatDau)
                return (false, "Giờ kết thúc phải sau giờ bắt đầu.");

            if (!await _repository.ExistsLopHocPhanAsync(
                lichHoc.MaLopHocPhan.Trim()))
                return (false, "Lớp học phần không tồn tại.");

            if (!string.IsNullOrWhiteSpace(lichHoc.MaPhong) &&
                !await _repository.ExistsPhongAsync(lichHoc.MaPhong.Trim()))
                return (false, "Phòng học không tồn tại.");

            if (await _repository.HasScheduleConflictAsync(
                lichHoc.MaLopHocPhan,
                lichHoc.MaPhong,
                lichHoc.NgayHoc,
                lichHoc.GioBatDau,
                lichHoc.GioKetThuc,
                maLichHoc))
                return (false, "Lịch học bị trùng giờ với lịch khác.");

            existing.MaLopHocPhan = lichHoc.MaLopHocPhan.Trim();
            existing.NgayHoc = lichHoc.NgayHoc;
            existing.GioBatDau = lichHoc.GioBatDau;
            existing.GioKetThuc = lichHoc.GioKetThuc;
            existing.MaPhong = string.IsNullOrWhiteSpace(lichHoc.MaPhong)
                ? null
                : lichHoc.MaPhong.Trim();
            existing.TrangThai = lichHoc.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật lịch học thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(
            int maLichHoc)
        {
            var existing = await _repository.GetByIdAsync(maLichHoc);

            if (existing == null)
                return (false, "Không tìm thấy lịch học.");

            await _repository.DeleteAsync(maLichHoc);
            await _repository.SaveChangesAsync();

            return (true, "Xóa lịch học thành công.");
        }
    }
}
