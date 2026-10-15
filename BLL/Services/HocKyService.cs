
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class HocKyService : IHocKyService
    {
        private readonly IHocKyRepository _repository;

        public HocKyService(IHocKyRepository repository)
        {
            _repository = repository;
        }

        public Task<List<HocKy>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<HocKy?> GetByIdAsync(string maHocKy)
        {
            return _repository.GetByIdAsync(maHocKy);
        }

        public Task<List<HocKy>> SearchAsync(string keyword)
        {
            return _repository.SearchAsync(keyword);
        }

        public async Task<(bool Success, string Message)> AddAsync(HocKy hocKy)
        {
            if (hocKy == null)
                return (false, "Dữ liệu học kỳ không hợp lệ.");

            if (string.IsNullOrWhiteSpace(hocKy.MaHocKy))
                return (false, "Mã học kỳ không được để trống.");

            if (string.IsNullOrWhiteSpace(hocKy.TenHocKy))
                return (false, "Tên học kỳ không được để trống.");

            if (string.IsNullOrWhiteSpace(hocKy.MaNienKhoa))
                return (false, "Mã niên khóa không được để trống.");

            if (hocKy.NgayKetThuc <= hocKy.NgayBatDau)
                return (false, "Ngày kết thúc phải sau ngày bắt đầu.");

            if (await _repository.ExistsAsync(hocKy.MaHocKy.Trim()))
                return (false, "Mã học kỳ đã tồn tại.");

            if (!await _repository.ExistsNienKhoaAsync(hocKy.MaNienKhoa.Trim()))
                return (false, "Niên khóa không tồn tại.");

            hocKy.MaHocKy = hocKy.MaHocKy.Trim();
            hocKy.TenHocKy = hocKy.TenHocKy.Trim();
            hocKy.MaNienKhoa = hocKy.MaNienKhoa.Trim();

            await _repository.AddAsync(hocKy);
            await _repository.SaveChangesAsync();

            return (true, "Thêm học kỳ thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            string maHocKy, HocKy hocKy)
        {
            if (hocKy == null)
                return (false, "Dữ liệu học kỳ không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maHocKy);

            if (existing == null)
                return (false, "Không tìm thấy học kỳ.");

            if (string.IsNullOrWhiteSpace(hocKy.TenHocKy))
                return (false, "Tên học kỳ không được để trống.");

            if (string.IsNullOrWhiteSpace(hocKy.MaNienKhoa))
                return (false, "Mã niên khóa không được để trống.");

            if (hocKy.NgayKetThuc <= hocKy.NgayBatDau)
                return (false, "Ngày kết thúc phải sau ngày bắt đầu.");

            if (!await _repository.ExistsNienKhoaAsync(hocKy.MaNienKhoa.Trim()))
                return (false, "Niên khóa không tồn tại.");

            existing.TenHocKy = hocKy.TenHocKy.Trim();
            existing.MaNienKhoa = hocKy.MaNienKhoa.Trim();
            existing.NgayBatDau = hocKy.NgayBatDau;
            existing.NgayKetThuc = hocKy.NgayKetThuc;
            existing.TrangThai = hocKy.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật học kỳ thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string maHocKy)
        {
            var existing = await _repository.GetByIdAsync(maHocKy);

            if (existing == null)
                return (false, "Không tìm thấy học kỳ.");

            await _repository.DeleteAsync(maHocKy);
            await _repository.SaveChangesAsync();

            return (true, "Xóa học kỳ thành công.");
        }
    }
}
