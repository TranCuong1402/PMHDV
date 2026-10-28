using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class DiemService : IDiemService
    {
        private readonly IDiemRepository _repository;

        public DiemService(IDiemRepository repository)
        {
            _repository = repository;
        }

        public async Task NhapDiemAsync(NhapDiemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.MaSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(request.MaLopHocPhan))
                throw new ArgumentException("Mã lớp học phần không được để trống.");

            if (!HopLe(request.DiemChuyenCan) ||
                !HopLe(request.DiemGiuaKy) ||
                !HopLe(request.DiemCuoiKy))
            {
                throw new ArgumentException(
                    "Điểm phải nằm trong khoảng từ 0 đến 10.");
            }

            if (!request.DiemChuyenCan.HasValue &&
                !request.DiemGiuaKy.HasValue &&
                !request.DiemCuoiKy.HasValue)
            {
                throw new ArgumentException(
                    "Bạn phải nhập ít nhất một đầu điểm.");
            }

            if (!await _repository.SinhVienExistsAsync(request.MaSV))
                throw new KeyNotFoundException("Không tìm thấy sinh viên.");

            if (!await _repository.LopHocPhanExistsAsync(request.MaLopHocPhan))
                throw new KeyNotFoundException("Không tìm thấy lớp học phần.");

            if (!await _repository.SinhVienDaDangKyAsync(
                request.MaSV, request.MaLopHocPhan))
            {
                throw new InvalidOperationException(
                    "Sinh viên chưa đăng ký lớp học phần này.");
            }

            if (await _repository.DiemExistsAsync(
                request.MaSV, request.MaLopHocPhan))
            {
                throw new InvalidOperationException(
                    "Sinh viên đã có bản ghi điểm. Hãy sử dụng UC31 để sửa điểm.");
            }

            var diem = new Diem
            {
                MaSV = request.MaSV,
                MaLopHocPhan = request.MaLopHocPhan,
                DiemChuyenCan = request.DiemChuyenCan,
                DiemGiuaKy = request.DiemGiuaKy,
                DiemCuoiKy = request.DiemCuoiKy,
                UpdatedAt = DateTime.Now
            };

            await _repository.AddAsync(diem);
            await _repository.SaveChangesAsync();
        }

        private static bool HopLe(decimal? diem)
        {
            return !diem.HasValue ||
                   (diem.Value >= 0 && diem.Value <= 10);
        }
    }
}