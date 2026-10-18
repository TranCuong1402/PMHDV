using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface ILopHocPhanService
    {
        Task<List<LopHocPhanDto>> GetAllAsync();
        Task<LopHocPhanDto?> GetByIdAsync(string maLopHocPhan);
        Task<List<LopHocPhanDto>> SearchAsync(string? keyword);
    }
}