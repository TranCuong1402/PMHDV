using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface IGiangVienRepository
    {
        Task<List<GiangVien>> GetAllAsync();
        Task<GiangVien?> GetByIdAsync(string maGiangVien);
        Task<List<GiangVien>> SearchAsync(string keyword);
        Task AddAsync(GiangVien giangVien);
        Task UpdateAsync(GiangVien giangVien);
        Task DeleteAsync(string maGiangVien);
        Task<bool> ExistsAsync(string maGiangVien);
        Task SaveChangesAsync();
    }
}