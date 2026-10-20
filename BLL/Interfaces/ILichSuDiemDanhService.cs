
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface ILichSuDiemDanhService
    {
        Task<List<LichSuDiemDanhDto>> GetBySinhVienAsync(
            string maSV);
    }
}
