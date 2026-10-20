
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILopHocPhanSinhVienRepository
    {
        Task<List<LopHocPhanSinhVienDto>> GetBySinhVienAsync(
            string maSV);

        Task<bool> SinhVienExistsAsync(string maSV);
    }
}
