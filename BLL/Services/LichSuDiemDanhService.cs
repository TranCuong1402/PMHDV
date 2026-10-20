using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public class LichSuDiemDanhService : ILichSuDiemDanhService
    {
        private readonly ILichSuDiemDanhRepository _repository;

        public LichSuDiemDanhService(
            ILichSuDiemDanhRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LichSuDiemDanhDto>> GetBySinhVienAsync(
            string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
            {
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");
            }

            maSV = maSV.Trim();

            bool tonTai = await _repository.SinhVienExistsAsync(maSV);

            if (!tonTai)
            {
                throw new ArgumentException(
                    "Sinh viên không tồn tại.");
            }

            return await _repository.GetBySinhVienAsync(maSV);
        }
    }
}
