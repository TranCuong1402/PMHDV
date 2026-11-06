
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class DiemDanhService : IDiemDanhService
    {
        private readonly IDiemDanhRepository _repository;

        private static readonly string[] TrangThaiHopLe =
        {
            "Có mặt", "Vắng", "Muộn", "Có phép"
        };

        public DiemDanhService(IDiemDanhRepository repository)
        {
            _repository = repository;
        }

        // Kiểm tra quyền giảng viên phụ trách lịch học
        private async Task KiemTraQuyenAsync(
            int maLichHoc,
            int idTaiKhoan,
            string vaiTro)
        {
            if (vaiTro == "Admin")
                return;

            if (vaiTro != "GiangVien")
                throw new UnauthorizedAccessException(
                    "Bạn không có quyền thực hiện chức năng này.");

            bool duocPhep = await _repository
                .GiangVienPhuTrachLichHocAsync(
                    idTaiKhoan,
                    maLichHoc);

            if (!duocPhep)
                throw new UnauthorizedAccessException(
                    "Bạn không được phân công giảng dạy lớp học này.");
        }

        public async Task<string> DiemDanhAsync(
            DiemDanhRequest request,
            int idTaiKhoan,
            string vaiTro)
        {
            if (request == null)
                throw new ArgumentException(
                    "Dữ liệu điểm danh không hợp lệ.");

            string maSV = request.MaSV?.Trim() ?? "";
            string trangThai = request.TrangThai?.Trim() ?? "";

            if (request.MaLichHoc <= 0)
                throw new ArgumentException(
                    "Mã lịch học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");

            if (!TrangThaiHopLe.Contains(trangThai))
                throw new ArgumentException(
                    "Trạng thái điểm danh không hợp lệ.");

            if (!await _repository.LichHocExistsAsync(
                request.MaLichHoc))
                throw new ArgumentException(
                    "Lịch học không tồn tại.");

            await KiemTraQuyenAsync(
                request.MaLichHoc,
                idTaiKhoan,
                vaiTro);

            if (!await _repository.SinhVienExistsAsync(maSV))
                throw new ArgumentException(
                    "Sinh viên không tồn tại hoặc đã bị khóa.");

            var existing =
                await _repository.GetByLichHocAndSinhVienAsync(
                    request.MaLichHoc, maSV);

            if (existing != null)
                throw new ArgumentException(
                    "Sinh viên đã được điểm danh ở lịch học này.");

            var diemDanh = new DiemDanh
            {
                MaLichHoc = request.MaLichHoc,
                MaSV = maSV,
                TrangThai = trangThai,
                ThoiGianDiemDanh = DateTime.Now,
                LyDo = request.LyDo?.Trim(),
                NguoiDiemDanh = request.NguoiDiemDanh?.Trim()
            };

            await _repository.AddAsync(diemDanh);
            await _repository.SaveChangesAsync();

            return "Điểm danh sinh viên thành công.";
        }

        public async Task<string> SuaDiemDanhAsync(
            int maLichHoc,
            string maSV,
            SuaDiemDanhRequest request,
            int idTaiKhoan,
            string vaiTro)
        {
            if (request == null)
                throw new ArgumentException(
                    "Dữ liệu sửa điểm danh không hợp lệ.");

            string trangThai = request.TrangThai?.Trim() ?? "";

            if (maLichHoc <= 0)
                throw new ArgumentException(
                    "Mã lịch học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");

            if (!TrangThaiHopLe.Contains(trangThai))
                throw new ArgumentException(
                    "Trạng thái điểm danh không hợp lệ.");

            if (!await _repository.LichHocExistsAsync(maLichHoc))
                throw new ArgumentException(
                    "Lịch học không tồn tại.");

            await KiemTraQuyenAsync(
                maLichHoc,
                idTaiKhoan,
                vaiTro);

            if (!await _repository.SinhVienExistsAsync(maSV))
                throw new ArgumentException(
                    "Sinh viên không tồn tại hoặc đã bị khóa.");

            var existing =
                await _repository.GetByLichHocAndSinhVienAsync(
                    maLichHoc, maSV);

            if (existing == null)
                throw new ArgumentException(
                    "Sinh viên chưa được điểm danh ở lịch học này.");

            existing.TrangThai = trangThai;
            existing.LyDo = request.LyDo?.Trim();

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return "Sửa điểm danh sinh viên thành công.";
        }
    }
}
