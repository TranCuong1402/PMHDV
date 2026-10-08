using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class GiangVienService : IGiangVienService
    {
        private readonly IGiangVienRepository _repository;

        public GiangVienService(IGiangVienRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GiangVien>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<GiangVien?> GetByIdAsync(string maGiangVien)
        {
            return await _repository.GetByIdAsync(maGiangVien);
        }

        public async Task<List<GiangVien>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await _repository.GetAllAsync();
            }

            return await _repository.SearchAsync(keyword.Trim());
        }

        public async Task<(bool Success, string Message)> AddAsync(
            GiangVien giangVien)
        {
            if (giangVien == null)
                return (false, "Dữ liệu giảng viên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(giangVien.MaGiangVien))
                return (false, "Mã giảng viên không được để trống.");

            if (string.IsNullOrWhiteSpace(giangVien.HoTen))
                return (false, "Họ tên giảng viên không được để trống.");

            var exists = await _repository.ExistsAsync(
                giangVien.MaGiangVien);

            if (exists)
                return (false, "Mã giảng viên đã tồn tại.");

            giangVien.MaGiangVien =
                giangVien.MaGiangVien.Trim();

            giangVien.HoTen =
                giangVien.HoTen.Trim();

            await _repository.AddAsync(giangVien);
            await _repository.SaveChangesAsync();

            return (true, "Thêm giảng viên thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            string maGiangVien,
            GiangVien giangVien)
        {
            if (string.IsNullOrWhiteSpace(maGiangVien))
                return (false, "Mã giảng viên không hợp lệ.");

            if (giangVien == null)
                return (false, "Dữ liệu giảng viên không hợp lệ.");

            var existing =
                await _repository.GetByIdAsync(maGiangVien);

            if (existing == null)
                return (false, "Không tìm thấy giảng viên.");

            if (string.IsNullOrWhiteSpace(giangVien.HoTen))
                return (false, "Họ tên giảng viên không được để trống.");

            existing.IdTaiKhoan = giangVien.IdTaiKhoan;
            existing.HoTen = giangVien.HoTen.Trim();
            existing.Email = giangVien.Email;
            existing.SDT = giangVien.SDT;
            existing.Khoa = giangVien.Khoa;
            existing.TrangThai = giangVien.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật giảng viên thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(
            string maGiangVien)
        {
            if (string.IsNullOrWhiteSpace(maGiangVien))
                return (false, "Mã giảng viên không hợp lệ.");

            var existing =
                await _repository.GetByIdAsync(maGiangVien);

            if (existing == null)
                return (false, "Không tìm thấy giảng viên.");

            await _repository.DeleteAsync(maGiangVien);
            await _repository.SaveChangesAsync();

            return (true, "Xóa giảng viên thành công.");
        }
    }
}