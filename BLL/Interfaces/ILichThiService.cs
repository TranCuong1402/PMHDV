using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface ILichThiService
    {
        Task<List<LichThi>> GetAllAsync();
        Task<LichThi?> GetByIdAsync(int maLichThi);
        Task<List<LichThi>> SearchAsync(string keyword);
        Task<(bool Success, string Message)> AddAsync(LichThi lichThi);
        Task<(bool Success, string Message)> UpdateAsync(
            int maLichThi, LichThi lichThi);
        Task<(bool Success, string Message)> DeleteAsync(int maLichThi);
    }
}
