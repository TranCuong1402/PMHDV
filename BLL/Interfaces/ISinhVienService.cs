using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface ISinhVienService
    {
        Task<List<SinhVien>> GetAllAsync();

        Task<SinhVien?> GetByIdAsync(string maSV);

        Task<List<SinhVien>> SearchAsync(string keyword);

        Task<(bool Success, string Message)> AddAsync(SinhVien sinhVien);

        Task<(bool Success, string Message)> UpdateAsync(
            string maSV,
            SinhVien sinhVien);

        Task<(bool Success, string Message)> DeleteAsync(string maSV);
    }
}