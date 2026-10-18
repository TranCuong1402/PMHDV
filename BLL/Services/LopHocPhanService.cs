using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public class LopHocPhanService : ILopHocPhanService
    {
        private readonly ILopHocPhanRepository _repository;

        public LopHocPhanService(ILopHocPhanRepository repository)
        {
            _repository = repository;
        }

        public Task<List<LopHocPhanDto>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public async Task<LopHocPhanDto?> GetByIdAsync(string maLopHocPhan)
        {
            if (string.IsNullOrWhiteSpace(maLopHocPhan))
                return null;

            return await _repository.GetByIdAsync(maLopHocPhan.Trim());
        }

        public async Task<List<LopHocPhanDto>> SearchAsync(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _repository.GetAllAsync();

            return await _repository.SearchAsync(keyword.Trim());
        }
    }
}