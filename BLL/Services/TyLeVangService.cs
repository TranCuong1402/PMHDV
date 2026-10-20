
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public class TyLeVangService : ITyLeVangService
    {
        private readonly ITyLeVangRepository _repository;

        public TyLeVangService(ITyLeVangRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TyLeVangDto>> TinhTyLeVangAsync(
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

            return await _repository.TinhTyLeVangAsync(maSV);
        }
    }
}
