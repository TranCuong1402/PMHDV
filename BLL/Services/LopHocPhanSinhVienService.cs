
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public class LopHocPhanSinhVienService
        : ILopHocPhanSinhVienService
    {
        private readonly ILopHocPhanSinhVienRepository _repository;

        public LopHocPhanSinhVienService(
            ILopHocPhanSinhVienRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LopHocPhanSinhVienDto>>
            GetBySinhVienAsync(string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");

            maSV = maSV.Trim();

            if (!await _repository.SinhVienExistsAsync(maSV))
                throw new KeyNotFoundException(
                    "Không tìm thấy sinh viên.");

            return await _repository.GetBySinhVienAsync(maSV);
        }
    }
}
