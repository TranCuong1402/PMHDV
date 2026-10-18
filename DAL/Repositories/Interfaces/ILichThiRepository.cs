
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILichThiRepository
    {
        Task<List<LichThi>> GetAllAsync();
        Task<LichThi?> GetByIdAsync(int maLichThi);
        Task<List<LichThi>> SearchAsync(string keyword);
        Task AddAsync(LichThi lichThi);
        Task UpdateAsync(LichThi lichThi);
        Task DeleteAsync(int maLichThi);
        Task<bool> ExistsLopHocPhanAsync(string maLopHocPhan);
        Task<bool> ExistsPhongAsync(string maPhong);
        Task<bool> HasScheduleConflictAsync(
            string maLopHocPhan,
            string? maPhong,
            DateTime ngayThi,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc,
            int? excludeMaLichThi = null);
        Task SaveChangesAsync();
    }
}
