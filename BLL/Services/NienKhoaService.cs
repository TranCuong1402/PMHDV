
using DAL.Repositories.Interfaces;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class NienKhoaService : INienKhoaService
    {
        private readonly INienKhoaRepository _repository;

        public NienKhoaService(INienKhoaRepository repository)
        {
            _repository = repository;
        }

        public Task<List<NienKhoa>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<NienKhoa?> GetByIdAsync(string maNienKhoa)
        {
            return _repository.GetByIdAsync(maNienKhoa);
        }

        public Task<List<NienKhoa>> SearchAsync(string keyword)
        {
            return _repository.SearchAsync(keyword);
        }

        public async Task<(bool Success, string Message)> AddAsync(
            NienKhoa nienKhoa)
        {
            if (nienKhoa == null)
                return (false, "Dữ liệu niên khóa không hợp lệ.");

            if (string.IsNullOrWhiteSpace(nienKhoa.MaNienKhoa))
                return (false, "Mã niên khóa không được để trống.");

            if (string.IsNullOrWhiteSpace(nienKhoa.TenNienKhoa))
                return (false, "Tên niên khóa không được để trống.");

            if (nienKhoa.NamBatDau <= 0 || nienKhoa.NamKetThuc <= nienKhoa.NamBatDau)
                return (false, "Năm kết thúc phải lớn hơn năm bắt đầu.");

            if (await _repository.ExistsAsync(nienKhoa.MaNienKhoa.Trim()))
                return (false, "Mã niên khóa đã tồn tại.");

            nienKhoa.MaNienKhoa = nienKhoa.MaNienKhoa.Trim();
            nienKhoa.TenNienKhoa = nienKhoa.TenNienKhoa.Trim();

            await _repository.AddAsync(nienKhoa);
            await _repository.SaveChangesAsync();

            return (true, "Thêm niên khóa thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            string maNienKhoa, NienKhoa nienKhoa)
        {
            if (nienKhoa == null)
                return (false, "Dữ liệu niên khóa không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maNienKhoa);

            if (existing == null)
                return (false, "Không tìm thấy niên khóa.");

            if (string.IsNullOrWhiteSpace(nienKhoa.TenNienKhoa))
                return (false, "Tên niên khóa không được để trống.");

            if (nienKhoa.NamBatDau <= 0 || nienKhoa.NamKetThuc <= nienKhoa.NamBatDau)
                return (false, "Năm kết thúc phải lớn hơn năm bắt đầu.");

            existing.TenNienKhoa = nienKhoa.TenNienKhoa.Trim();
            existing.NamBatDau = nienKhoa.NamBatDau;
            existing.NamKetThuc = nienKhoa.NamKetThuc;
            existing.TrangThai = nienKhoa.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật niên khóa thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(
            string maNienKhoa)
        {
            var existing = await _repository.GetByIdAsync(maNienKhoa);

            if (existing == null)
                return (false, "Không tìm thấy niên khóa.");

            await _repository.DeleteAsync(maNienKhoa);
            await _repository.SaveChangesAsync();

            return (true, "Xóa niên khóa thành công.");
        }
    }
}
