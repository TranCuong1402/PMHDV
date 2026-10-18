
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class LichThiService : ILichThiService
    {
        private readonly ILichThiRepository _repository;

        public LichThiService(ILichThiRepository repository)
        {
            _repository = repository;
        }

        public Task<List<LichThi>> GetAllAsync()
            => _repository.GetAllAsync();

        public Task<LichThi?> GetByIdAsync(int maLichThi)
            => _repository.GetByIdAsync(maLichThi);

        public Task<List<LichThi>> SearchAsync(string keyword)
            => _repository.SearchAsync(keyword);

        public async Task<(bool Success, string Message)> AddAsync(
            LichThi lichThi)
        {
            if (lichThi == null)
                return (false, "Dữ liệu lịch thi không hợp lệ.");

            if (string.IsNullOrWhiteSpace(lichThi.MaLopHocPhan))
                return (false, "Mã lớp học phần không được để trống.");

            if (lichThi.GioKetThuc <= lichThi.GioBatDau)
                return (false, "Giờ kết thúc phải sau giờ bắt đầu.");

            if (!await _repository.ExistsLopHocPhanAsync(
                lichThi.MaLopHocPhan.Trim()))
            {
                return (false, "Lớp học phần không tồn tại.");
            }

            if (!string.IsNullOrWhiteSpace(lichThi.MaPhong) &&
                !await _repository.ExistsPhongAsync(lichThi.MaPhong.Trim()))
            {
                return (false, "Phòng thi không tồn tại.");
            }

            if (await _repository.HasScheduleConflictAsync(
                lichThi.MaLopHocPhan,
                lichThi.MaPhong,
                lichThi.NgayThi,
                lichThi.GioBatDau,
                lichThi.GioKetThuc))
            {
                return (false, "Lịch thi bị trùng giờ với lịch thi khác.");
            }

            lichThi.MaLopHocPhan = lichThi.MaLopHocPhan.Trim();
            lichThi.MaPhong = string.IsNullOrWhiteSpace(lichThi.MaPhong)
                ? null
                : lichThi.MaPhong.Trim();

            await _repository.AddAsync(lichThi);
            await _repository.SaveChangesAsync();

            return (true, "Thêm lịch thi thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            int maLichThi, LichThi lichThi)
        {
            if (lichThi == null)
                return (false, "Dữ liệu lịch thi không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maLichThi);

            if (existing == null)
                return (false, "Không tìm thấy lịch thi.");

            if (string.IsNullOrWhiteSpace(lichThi.MaLopHocPhan))
                return (false, "Mã lớp học phần không được để trống.");

            if (lichThi.GioKetThuc <= lichThi.GioBatDau)
                return (false, "Giờ kết thúc phải sau giờ bắt đầu.");

            if (!await _repository.ExistsLopHocPhanAsync(
                lichThi.MaLopHocPhan.Trim()))
            {
                return (false, "Lớp học phần không tồn tại.");
            }

            if (!string.IsNullOrWhiteSpace(lichThi.MaPhong) &&
                !await _repository.ExistsPhongAsync(lichThi.MaPhong.Trim()))
            {
                return (false, "Phòng thi không tồn tại.");
            }

            if (await _repository.HasScheduleConflictAsync(
                lichThi.MaLopHocPhan,
                lichThi.MaPhong,
                lichThi.NgayThi,
                lichThi.GioBatDau,
                lichThi.GioKetThuc,
                maLichThi))
            {
                return (false, "Lịch thi bị trùng giờ với lịch thi khác.");
            }

            existing.MaLopHocPhan = lichThi.MaLopHocPhan.Trim();
            existing.NgayThi = lichThi.NgayThi;
            existing.GioBatDau = lichThi.GioBatDau;
            existing.GioKetThuc = lichThi.GioKetThuc;
            existing.MaPhong = string.IsNullOrWhiteSpace(lichThi.MaPhong)
                ? null
                : lichThi.MaPhong.Trim();
            existing.HinhThucThi = lichThi.HinhThucThi;
            existing.TrangThai = lichThi.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật lịch thi thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(
            int maLichThi)
        {
            var existing = await _repository.GetByIdAsync(maLichThi);

            if (existing == null)
                return (false, "Không tìm thấy lịch thi.");

            await _repository.DeleteAsync(maLichThi);
            await _repository.SaveChangesAsync();

            return (true, "Xóa lịch thi thành công.");
        }
    }
}
