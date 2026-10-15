
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface IHocKyService
    {
        Task<List<HocKy>> GetAllAsync();
        Task<HocKy?> GetByIdAsync(string maHocKy);
        Task<List<HocKy>> SearchAsync(string keyword);
        Task<(bool Success, string Message)> AddAsync(HocKy hocKy);
        Task<(bool Success, string Message)> UpdateAsync(
            string maHocKy, HocKy hocKy);
        Task<(bool Success, string Message)> DeleteAsync(string maHocKy);
    }
}
