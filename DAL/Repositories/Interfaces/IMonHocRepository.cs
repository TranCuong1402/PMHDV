using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface IMonHocRepository
    {
        Task<List<MonHoc>> GetAllAsync();
        Task<MonHoc?> GetByIdAsync(string maMonHoc);
        Task<List<MonHoc>> SearchAsync(string keyword);
        Task AddAsync(MonHoc monHoc);
        Task UpdateAsync(MonHoc monHoc);
        Task DeleteAsync(string maMonHoc);
        Task<bool> ExistsAsync(string maMonHoc);
        Task SaveChangesAsync();
    }
}