
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILichSuDiemDanhRepository
    {
        Task<bool> SinhVienExistsAsync(string maSV);

        Task<List<LichSuDiemDanhDto>> GetBySinhVienAsync(
            string maSV);
    }
}
