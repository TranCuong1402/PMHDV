using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILopHocPhanRepository
    {
        Task<List<LopHocPhanDto>> GetAllAsync();
        Task<LopHocPhanDto?> GetByIdAsync(string maLopHocPhan);
        Task<List<LopHocPhanDto>> SearchAsync(string keyword);
    }
}