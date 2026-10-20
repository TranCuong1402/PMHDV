
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface IDiemDanhRepository
    {
        Task<bool> LichHocExistsAsync(int maLichHoc);
        Task<bool> SinhVienExistsAsync(string maSV);
        Task<DiemDanh?> GetByLichHocAndSinhVienAsync(
            int maLichHoc, string maSV);
        Task AddAsync(DiemDanh diemDanh);
        Task UpdateAsync(DiemDanh diemDanh);

        Task SaveChangesAsync();
    }
}
