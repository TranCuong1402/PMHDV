using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface IMonHocService
    {
        Task<List<MonHoc>> GetAllAsync();
        Task<MonHoc?> GetByIdAsync(string maMonHoc);
        Task<List<MonHoc>> SearchAsync(string keyword);

        Task<(bool Success, string Message)> AddAsync(MonHoc monHoc);

        Task<(bool Success, string Message)> UpdateAsync(
            string maMonHoc,
            MonHoc monHoc);

        Task<(bool Success, string Message)> DeleteAsync(
            string maMonHoc);
    }
}