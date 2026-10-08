using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface IGiangVienService
    {
        Task<List<GiangVien>> GetAllAsync();
        Task<GiangVien?> GetByIdAsync(string maGiangVien);
        Task<List<GiangVien>> SearchAsync(string keyword);

        Task<(bool Success, string Message)> AddAsync(GiangVien giangVien);

        Task<(bool Success, string Message)> UpdateAsync(
            string maGiangVien,
            GiangVien giangVien);

        Task<(bool Success, string Message)> DeleteAsync(
            string maGiangVien);
    }
}