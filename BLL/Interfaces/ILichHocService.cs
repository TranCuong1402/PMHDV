
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface ILichHocService
    {
        Task<List<LichHoc>> GetAllAsync();
        Task<LichHoc?> GetByIdAsync(int maLichHoc);
        Task<List<LichHoc>> SearchAsync(string keyword);
        Task<(bool Success, string Message)> AddAsync(LichHoc lichHoc);
        Task<(bool Success, string Message)> UpdateAsync(
            int maLichHoc, LichHoc lichHoc);
        Task<(bool Success, string Message)> DeleteAsync(int maLichHoc);
        Task SuaLichDayAsync(int maLichHoc, SuaLichDayRequest request);
        Task ChonPhongDayAsync(int maLichHoc,ChonPhongDayRequest request);
        Task<KiemTraTrungLichResponse> KiemTraTrungLichAsync(KiemTraTrungLichRequest request);
    }
}
