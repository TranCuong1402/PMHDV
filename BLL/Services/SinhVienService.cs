using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class SinhVienService : ISinhVienService
    {
        private readonly ISinhVienRepository _repository;

        public SinhVienService(ISinhVienRepository repository)
        {
            _repository = repository;
        }

        // ============================
        // LẤY DANH SÁCH
        // ============================
        public async Task<List<SinhVien>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // ============================
        // LẤY THEO MÃ SINH VIÊN
        // ============================
        public async Task<SinhVien?> GetByIdAsync(string maSV)
        {
            return await _repository.GetByIdAsync(maSV);
        }

        // ============================
        // TÌM KIẾM
        // ============================
        public async Task<List<SinhVien>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await _repository.GetAllAsync();
            }

            return await _repository.SearchAsync(keyword.Trim());
        }

        // ============================
        // THÊM
        // ============================
        public async Task<(bool Success, string Message)> AddAsync(
            SinhVien sinhVien)
        {
            if (sinhVien == null)
            {
                return (false, "Dữ liệu sinh viên không hợp lệ.");
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MaSV))
            {
                return (false, "Mã sinh viên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sinhVien.HoTen))
            {
                return (false, "Họ tên sinh viên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MaLop))
            {
                return (false, "Mã lớp không được để trống.");
            }

            var exists = await _repository.ExistsAsync(sinhVien.MaSV);

            if (exists)
            {
                return (false, "Mã sinh viên đã tồn tại.");
            }

            sinhVien.MaSV = sinhVien.MaSV.Trim();
            sinhVien.HoTen = sinhVien.HoTen.Trim();

            await _repository.AddAsync(sinhVien);
            await _repository.SaveChangesAsync();

            return (true, "Thêm sinh viên thành công.");
        }

        // ============================
        // CẬP NHẬT
        // ============================
        public async Task<(bool Success, string Message)> UpdateAsync(
            string maSV,
            SinhVien sinhVien)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                return (false, "Mã sinh viên không hợp lệ.");
            }

            if (sinhVien == null)
            {
                return (false, "Dữ liệu sinh viên không hợp lệ.");
            }

            var existing = await _repository.GetByIdAsync(maSV);

            if (existing == null)
            {
                return (false, "Không tìm thấy sinh viên.");
            }

            if (string.IsNullOrWhiteSpace(sinhVien.HoTen))
            {
                return (false, "Họ tên sinh viên không được để trống.");
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MaLop))
            {
                return (false, "Mã lớp không được để trống.");
            }

            // Không cho thay đổi MaSV
            existing.HoTen = sinhVien.HoTen.Trim();
            existing.IdTaiKhoan = sinhVien.IdTaiKhoan;
            existing.NgaySinh = sinhVien.NgaySinh;
            existing.GioiTinh = sinhVien.GioiTinh;
            existing.Email = sinhVien.Email;
            existing.SDT = sinhVien.SDT;
            existing.DiaChi = sinhVien.DiaChi;
            existing.MaLop = sinhVien.MaLop;
            existing.TrangThai = sinhVien.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật sinh viên thành công.");
        }

        // ============================
        // XÓA
        // ============================
        public async Task<(bool Success, string Message)> DeleteAsync(
            string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                return (false, "Mã sinh viên không hợp lệ.");
            }

            var existing = await _repository.GetByIdAsync(maSV);

            if (existing == null)
            {
                return (false, "Không tìm thấy sinh viên.");
            }

            await _repository.DeleteAsync(maSV);
            await _repository.SaveChangesAsync();

            return (true, "Xóa sinh viên thành công.");
        }
    }
}