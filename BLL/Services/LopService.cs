using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class LopService : ILopService
    {
        private readonly ILopRepository _repository;

        public LopService(ILopRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Lop>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Lop?> GetByIdAsync(string maLop)
        {
            return await _repository.GetByIdAsync(maLop);
        }

        public async Task<List<Lop>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await _repository.GetAllAsync();
            }

            return await _repository.SearchAsync(keyword.Trim());
        }

        public async Task<(bool Success, string Message)> AddAsync(Lop lop)
        {
            if (lop == null)
                return (false, "Dữ liệu lớp không hợp lệ.");

            if (string.IsNullOrWhiteSpace(lop.MaLop))
                return (false, "Mã lớp không được để trống.");

            if (string.IsNullOrWhiteSpace(lop.TenLop))
                return (false, "Tên lớp không được để trống.");

            if (string.IsNullOrWhiteSpace(lop.MaNienKhoa))
                return (false, "Mã niên khóa không được để trống.");

            var exists = await _repository.ExistsAsync(lop.MaLop);

            if (exists)
                return (false, "Mã lớp đã tồn tại.");

            var existsNienKhoa =
                await _repository.ExistsNienKhoaAsync(lop.MaNienKhoa);

            if (!existsNienKhoa)
                return (false, "Niên khóa không tồn tại.");

            lop.MaLop = lop.MaLop.Trim();
            lop.TenLop = lop.TenLop.Trim();
            lop.MaNienKhoa = lop.MaNienKhoa.Trim();

            if (!string.IsNullOrWhiteSpace(lop.Khoa))
                lop.Khoa = lop.Khoa.Trim();

            await _repository.AddAsync(lop);
            await _repository.SaveChangesAsync();

            return (true, "Thêm lớp thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            string maLop,
            Lop lop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
                return (false, "Mã lớp không hợp lệ.");

            if (lop == null)
                return (false, "Dữ liệu lớp không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maLop);

            if (existing == null)
                return (false, "Không tìm thấy lớp.");

            if (string.IsNullOrWhiteSpace(lop.TenLop))
                return (false, "Tên lớp không được để trống.");

            if (string.IsNullOrWhiteSpace(lop.MaNienKhoa))
                return (false, "Mã niên khóa không được để trống.");

            var existsNienKhoa =
                await _repository.ExistsNienKhoaAsync(lop.MaNienKhoa);

            if (!existsNienKhoa)
                return (false, "Niên khóa không tồn tại.");

            existing.TenLop = lop.TenLop.Trim();
            existing.MaNienKhoa = lop.MaNienKhoa.Trim();
            existing.Khoa = string.IsNullOrWhiteSpace(lop.Khoa)
                ? null
                : lop.Khoa.Trim();

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật lớp thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(
            string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
                return (false, "Mã lớp không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maLop);

            if (existing == null)
                return (false, "Không tìm thấy lớp.");

            await _repository.DeleteAsync(maLop);
            await _repository.SaveChangesAsync();

            return (true, "Xóa lớp thành công.");
        }
    }
}