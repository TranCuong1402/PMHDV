
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILichHocRepository
    {
        Task<List<LichHoc>> GetAllAsync();
        Task<LichHoc?> GetByIdAsync(int maLichHoc);
        Task<List<LichHoc>> SearchAsync(string keyword);
        Task AddAsync(LichHoc lichHoc);
        Task UpdateAsync(LichHoc lichHoc);
        Task DeleteAsync(int maLichHoc);
        Task<bool> ExistsAsync(int maLichHoc);
        Task<bool> ExistsLopHocPhanAsync(string maLopHocPhan);
        Task<bool> ExistsPhongAsync(string maPhong);
        Task<bool> HasScheduleConflictAsync(
            string maLopHocPhan,
            string? maPhong,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc,
            int? excludeMaLichHoc = null);

        Task<string?> GetMaGiangVienByLichHocAsync(int maLichHoc);

        Task<bool> HasScheduleConflictForUpdateAsync(
            int maLichHoc,
            string maLopHocPhan,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc);

        Task<bool> PhongExistsAsync(string maPhong);

        Task<bool> HasRoomConflictAsync(
            int maLichHoc,
            string maPhong,
            DateTime ngayHoc,
            TimeSpan gioBatDau,
            TimeSpan gioKetThuc);

        Task<KiemTraTrungLichResponse> KiemTraTrungLichAsync(
            KiemTraTrungLichRequest request);

        Task SaveChangesAsync();
    }
}
