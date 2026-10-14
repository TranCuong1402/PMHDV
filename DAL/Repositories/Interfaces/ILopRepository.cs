using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILopRepository
    {
        Task<List<Lop>> GetAllAsync();
        Task<Lop?> GetByIdAsync(string maLop);
        Task<List<Lop>> SearchAsync(string keyword);
        Task AddAsync(Lop lop);
        Task UpdateAsync(Lop lop);
        Task DeleteAsync(string maLop);
        Task<bool> ExistsAsync(string maLop);
        Task<bool> ExistsNienKhoaAsync(string maNienKhoa);
        Task SaveChangesAsync();
    }
}