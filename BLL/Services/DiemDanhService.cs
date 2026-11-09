
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
        public async Task<List<DiemDanh>> GetAllAsync(
    int idTaiKhoan,
    string vaiTro)
        {
            if (vaiTro == "Admin")
            {
                return await _repository.GetAllAsync();
            }

            if (vaiTro == "GiangVien")
            {
                return await _repository.GetByGiangVienAsync(
                    idTaiKhoan);
            }

            throw new UnauthorizedAccessException(
                "Bạn không có quyền xem danh sách điểm danh.");
        }

        public async Task<List<DiemDanh>> GetByLichHocAsync(
            int maLichHoc,
            int idTaiKhoan,
            string vaiTro)
        {
            if (maLichHoc <= 0)
                throw new ArgumentException(
                    "Mã lịch học không hợp lệ.");

            if (!await _repository.LichHocExistsAsync(maLichHoc))
                throw new ArgumentException(
                    "Lịch học không tồn tại.");

            await KiemTraQuyenAsync(
                maLichHoc,
                idTaiKhoan,
                vaiTro);

            return await _repository.GetByLichHocAsync(maLichHoc);
        }
        public async Task<string> XoaDiemDanhAsync(
    int maLichHoc,
    string maSV,
    int idTaiKhoan,
    string vaiTro)
        {
            maSV = maSV?.Trim() ?? "";

            // Kiểm tra mã lịch học
            if (maLichHoc <= 0)
                throw new ArgumentException(
                    "Mã lịch học không hợp lệ.");

            // Kiểm tra mã sinh viên
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");

            // Kiểm tra lịch học tồn tại
            if (!await _repository.LichHocExistsAsync(maLichHoc))
                throw new ArgumentException(
                    "Lịch học không tồn tại.");

            // Kiểm tra quyền Admin hoặc Giảng viên
            await KiemTraQuyenAsync(
                maLichHoc,
                idTaiKhoan,
                vaiTro);

            // Tìm bản ghi điểm danh
            var existing =
                await _repository.GetByLichHocAndSinhVienAsync(
                    maLichHoc,
                    maSV);

            if (existing == null)
                throw new ArgumentException(
                    "Không tìm thấy bản ghi điểm danh cần xóa.");

            // Xóa bản ghi
            await _repository.DeleteAsync(existing);
            await _repository.SaveChangesAsync();

            return "Xóa bản ghi điểm danh thành công.";
        }
        public async Task<List<SinhVienDiemDanhDto>>
    LayDanhSachSinhVienTheoLichHocAsync(
        int maLichHoc,
        int idTaiKhoan,
        string vaiTro)
        {
            if (maLichHoc <= 0)
            {
                throw new ArgumentException("Mã lịch học không hợp lệ.");
            }

            // Kiểm tra lịch học tồn tại
            bool lichHocTonTai =
                await _repository.LichHocExistsAsync(maLichHoc);

            if (!lichHocTonTai)
            {
                throw new Exception("Lịch học không tồn tại.");
            }

            // Kiểm tra quyền Admin / Giảng viên
            await KiemTraQuyenAsync(maLichHoc, idTaiKhoan, vaiTro);

            // Lấy danh sách sinh viên đã đăng ký
            return await _repository
                .GetDanhSachSinhVienTheoLichHocAsync(maLichHoc);
        }
    }
}
