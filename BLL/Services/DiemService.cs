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
    public async Task SuaDiemAsync(
    string maSV,
    string maLopHocPhan,
    SuaDiemRequest request)
        {
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(maLopHocPhan))
                throw new ArgumentException("Mã lớp học phần không được để trống.");

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!HopLe(request.DiemChuyenCan) ||
                !HopLe(request.DiemGiuaKy) ||
                !HopLe(request.DiemCuoiKy))
            {
                throw new ArgumentException("Điểm phải nằm trong khoảng 0 đến 10.");
            }

            var diem = await _repository.GetBySinhVienAndLopHocPhanAsync(
                maSV, maLopHocPhan);

            if (diem == null)
                throw new KeyNotFoundException("Không tìm thấy bản ghi điểm.");

            diem.DiemChuyenCan = request.DiemChuyenCan;
            diem.DiemGiuaKy = request.DiemGiuaKy;
            diem.DiemCuoiKy = request.DiemCuoiKy;
            diem.UpdatedAt = DateTime.Now;

            await _repository.SaveChangesAsync();
        }
        public async Task<DiemTongKetDto> TinhDiemTongKetAsync(
            string maSV,
            string maLopHocPhan)
        {
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(maLopHocPhan))
                throw new ArgumentException("Mã lớp học phần không được để trống.");

            var diem = await _repository.GetBySinhVienAndLopHocPhanAsync(
                maSV, maLopHocPhan);

            if (diem == null)
                throw new KeyNotFoundException("Không tìm thấy bản ghi điểm.");

            if (!diem.DiemChuyenCan.HasValue ||
                !diem.DiemGiuaKy.HasValue ||
                !diem.DiemCuoiKy.HasValue)
            {
                throw new InvalidOperationException(
                    "Sinh viên chưa có đủ ba đầu điểm để tính tổng kết.");
            }

            // Công thức minh họa: 10% chuyên cần, 30% giữa kỳ, 60% cuối kỳ.
            decimal tongKet =
                diem.DiemChuyenCan.Value * 0.1m +
                diem.DiemGiuaKy.Value * 0.3m +
                diem.DiemCuoiKy.Value * 0.6m;

            tongKet = Math.Round(tongKet, 2, MidpointRounding.AwayFromZero);

            string xepLoai;

            if (tongKet >= 8.5m)
                xepLoai = "Giỏi";
            else if (tongKet >= 7.0m)
                xepLoai = "Khá";
            else if (tongKet >= 5.0m)
                xepLoai = "Trung bình";
            else
                xepLoai = "Yếu";

            diem.DiemTongKet = tongKet;
            diem.XepLoai = xepLoai;
            diem.UpdatedAt = DateTime.Now;

            await _repository.SaveChangesAsync();

            return new DiemTongKetDto
            {
                MaSV = diem.MaSV,
                MaLopHocPhan = diem.MaLopHocPhan,
                DiemChuyenCan = diem.DiemChuyenCan,
                DiemGiuaKy = diem.DiemGiuaKy,
                DiemCuoiKy = diem.DiemCuoiKy,
                DiemTongKet = diem.DiemTongKet,
                XepLoai = diem.XepLoai
            };
        }

        public async Task<List<XemDiemDto>> XemDiemSinhVienAsync(
            string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");

            var sinhVienTonTai =
                await _repository.SinhVienExistsAsync(maSV);

            if (!sinhVienTonTai)
                throw new KeyNotFoundException(
                    "Không tìm thấy sinh viên.");

            return await _repository.GetDiemBySinhVienAsync(maSV);
        }



    }

}