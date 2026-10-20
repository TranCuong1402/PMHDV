
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface IDiemDanhService
    {
        Task<string> DiemDanhAsync(DiemDanhRequest request);
        Task<string> SuaDiemDanhAsync(
            int maLichHoc,
            string maSV,
            SuaDiemDanhRequest request);
    }
}
