using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ISinhVienRepository
    {
        Task<List<SinhVien>> GetAllAsync();

        Task<SinhVien?> GetByIdAsync(string maSV);

        Task<List<SinhVien>> SearchAsync(string keyword);

        Task AddAsync(SinhVien sinhVien);

        Task UpdateAsync(SinhVien sinhVien);

        Task DeleteAsync(string maSV);

        Task<bool> ExistsAsync(string maSV);

        Task SaveChangesAsync();
    }
}