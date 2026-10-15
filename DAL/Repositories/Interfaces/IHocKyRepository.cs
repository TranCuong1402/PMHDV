
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface IHocKyRepository
    {
        Task<List<HocKy>> GetAllAsync();
        Task<HocKy?> GetByIdAsync(string maHocKy);
        Task<List<HocKy>> SearchAsync(string keyword);
        Task AddAsync(HocKy hocKy);
        Task UpdateAsync(HocKy hocKy);
        Task DeleteAsync(string maHocKy);
        Task<bool> ExistsAsync(string maHocKy);
        Task<bool> ExistsNienKhoaAsync(string maNienKhoa);
        Task SaveChangesAsync();
    }
}
