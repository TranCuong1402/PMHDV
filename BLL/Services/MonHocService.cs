using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class MonHocService : IMonHocService
    {
        private readonly IMonHocRepository _repository;

        public MonHocService(IMonHocRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MonHoc>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<MonHoc?> GetByIdAsync(string maMonHoc)
        {
            return await _repository.GetByIdAsync(maMonHoc);
        }

        public async Task<List<MonHoc>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await _repository.GetAllAsync();
            }

            return await _repository.SearchAsync(keyword.Trim());
        }

        public async Task<(bool Success, string Message)> AddAsync(
            MonHoc monHoc)
        {
            if (monHoc == null)
                return (false, "Dữ liệu môn học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(monHoc.MaMonHoc))
                return (false, "Mã môn học không được để trống.");

            if (string.IsNullOrWhiteSpace(monHoc.TenMonHoc))
                return (false, "Tên môn học không được để trống.");

            if (monHoc.SoTinChi <= 0)
                return (false, "Số tín chỉ phải lớn hơn 0.");

            var exists = await _repository.ExistsAsync(
                monHoc.MaMonHoc);

            if (exists)
                return (false, "Mã môn học đã tồn tại.");

            monHoc.MaMonHoc = monHoc.MaMonHoc.Trim();
            monHoc.TenMonHoc = monHoc.TenMonHoc.Trim();

            if (!string.IsNullOrWhiteSpace(monHoc.MoTa))
            {
                monHoc.MoTa = monHoc.MoTa.Trim();
            }

            await _repository.AddAsync(monHoc);
            await _repository.SaveChangesAsync();

            return (true, "Thêm môn học thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            string maMonHoc,
            MonHoc monHoc)
        {
            if (string.IsNullOrWhiteSpace(maMonHoc))
                return (false, "Mã môn học không hợp lệ.");

            if (monHoc == null)
                return (false, "Dữ liệu môn học không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maMonHoc);

            if (existing == null)
                return (false, "Không tìm thấy môn học.");

            if (string.IsNullOrWhiteSpace(monHoc.TenMonHoc))
                return (false, "Tên môn học không được để trống.");

            if (monHoc.SoTinChi <= 0)
                return (false, "Số tín chỉ phải lớn hơn 0.");

            existing.TenMonHoc = monHoc.TenMonHoc.Trim();
            existing.SoTinChi = monHoc.SoTinChi;
            existing.MoTa = string.IsNullOrWhiteSpace(monHoc.MoTa)
                ? null
                : monHoc.MoTa.Trim();
            existing.TrangThai = monHoc.TrangThai;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return (true, "Cập nhật môn học thành công.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(
            string maMonHoc)
        {
            if (string.IsNullOrWhiteSpace(maMonHoc))
                return (false, "Mã môn học không hợp lệ.");

            var existing = await _repository.GetByIdAsync(maMonHoc);

            if (existing == null)
                return (false, "Không tìm thấy môn học.");

            await _repository.DeleteAsync(maMonHoc);
            await _repository.SaveChangesAsync();

            return (true, "Xóa môn học thành công.");
        }
    }
}