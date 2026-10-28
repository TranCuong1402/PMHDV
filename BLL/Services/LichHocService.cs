
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;
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

        public async Task SuaLichDayAsync(
            int maLichHoc,
            SuaLichDayRequest request)
        {
            if (maLichHoc <= 0)
                throw new ArgumentException("Mã lịch học không hợp lệ.");

            if (request == null)
                throw new ArgumentException("Dữ liệu cập nhật không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.MaGiangVien))
                throw new ArgumentException("Mã giảng viên không được để trống.");

            if (request.GioBatDau < TimeSpan.Zero ||
                request.GioKetThuc > TimeSpan.FromDays(1) ||
                request.GioBatDau >= request.GioKetThuc)
            {
                throw new ArgumentException(
                    "Giờ bắt đầu và kết thúc không hợp lệ.");
            }

            var lichHoc = await _repository.GetByIdAsync(maLichHoc);

            if (lichHoc == null)
                throw new ArgumentException("Lịch học không tồn tại.");

            var maGiangVienDuocPhanCong =
                await _repository.GetMaGiangVienByLichHocAsync(maLichHoc);

            if (maGiangVienDuocPhanCong == null ||
                maGiangVienDuocPhanCong != request.MaGiangVien.Trim())
            {
                throw new ArgumentException(
                    "Bạn không được phân công giảng dạy lớp học phần này.");
            }

            bool biTrung = await _repository.HasScheduleConflictForUpdateAsync(
                maLichHoc,
                lichHoc.MaLopHocPhan,
                request.NgayHoc,
                request.GioBatDau,
                request.GioKetThuc);

            if (biTrung)
                throw new ArgumentException(
                    "Lịch mới bị trùng với lịch học khác của lớp hoặc giảng viên.");

            lichHoc.NgayHoc = request.NgayHoc.Date;
            lichHoc.GioBatDau = request.GioBatDau;
            lichHoc.GioKetThuc = request.GioKetThuc;

            if (!string.IsNullOrWhiteSpace(request.TrangThai))
                lichHoc.TrangThai = request.TrangThai.Trim();

            await _repository.UpdateAsync(lichHoc);
            await _repository.SaveChangesAsync();
        }

        public async Task ChonPhongDayAsync(
            int maLichHoc,
            ChonPhongDayRequest request)
        {
            if (maLichHoc <= 0)
                throw new ArgumentException("Mã lịch học không hợp lệ.");

            if (request == null ||
                string.IsNullOrWhiteSpace(request.MaGiangVien) ||
                string.IsNullOrWhiteSpace(request.MaPhong))
            {
                throw new ArgumentException(
                    "Mã giảng viên và mã phòng không được để trống.");
            }

            string maGiangVien = request.MaGiangVien.Trim();
            string maPhong = request.MaPhong.Trim();

            var lichHoc = await _repository.GetByIdAsync(maLichHoc);

            if (lichHoc == null)
                throw new ArgumentException("Lịch học không tồn tại.");

            var maGiangVienDuocPhanCong =
                await _repository.GetMaGiangVienByLichHocAsync(maLichHoc);

            if (maGiangVienDuocPhanCong == null ||
                maGiangVienDuocPhanCong != maGiangVien)
            {
                throw new ArgumentException(
                    "Giảng viên không được phân công dạy lớp học phần này.");
            }

            bool phongTonTai = await _repository.PhongExistsAsync(maPhong);

            if (!phongTonTai)
            {
                throw new ArgumentException(
                    "Phòng học không tồn tại hoặc đang không hoạt động.");
            }

            bool biTrung = await _repository.HasRoomConflictAsync(
                maLichHoc,
                maPhong,
                lichHoc.NgayHoc,
                lichHoc.GioBatDau,
                lichHoc.GioKetThuc);

            if (biTrung)
            {
                throw new ArgumentException(
                    "Phòng học đã được sử dụng trong khung giờ này.");
            }

            lichHoc.MaPhong = maPhong;

            await _repository.UpdateAsync(lichHoc);
            await _repository.SaveChangesAsync();
        }

        public async Task<KiemTraTrungLichResponse> KiemTraTrungLichAsync(
            KiemTraTrungLichRequest request)
        {
            if (request == null)
                throw new ArgumentException("Dữ liệu kiểm tra không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.MaLopHocPhan))
                throw new ArgumentException(
                    "Mã lớp học phần không được để trống.");

            if (request.GioBatDau < TimeSpan.Zero ||
                request.GioKetThuc > TimeSpan.FromDays(1) ||
                request.GioBatDau >= request.GioKetThuc)
            {
                throw new ArgumentException(
                    "Giờ bắt đầu và giờ kết thúc không hợp lệ.");
            }

            if (request.MaLichHoc.HasValue)
            {
                if (request.MaLichHoc.Value <= 0)
                    throw new ArgumentException("Mã lịch học không hợp lệ.");

                var lichHienTai = await _repository
                    .GetByIdAsync(request.MaLichHoc.Value);

                if (lichHienTai == null)
                    throw new ArgumentException("Lịch học cần kiểm tra không tồn tại.");

                if (lichHienTai.MaLopHocPhan != request.MaLopHocPhan)
                    throw new ArgumentException(
                        "Mã lớp học phần không khớp với lịch học cần cập nhật.");
            }

            return await _repository.KiemTraTrungLichAsync(request);
        }

    }
}
