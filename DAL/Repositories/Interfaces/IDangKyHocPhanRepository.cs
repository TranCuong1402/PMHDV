
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface IDangKyHocPhanRepository
    {
        Task<bool> SinhVienExistsAsync(string maSV);
        Task<bool> LopHocPhanExistsAsync(string maLopHocPhan);
        Task<bool> DaDangKyAsync(string maSV, string maLopHocPhan);
        Task<int> DemSoLuongDangKyAsync(string maLopHocPhan);
        Task<int?> LaySiSoToiDaAsync(string maLopHocPhan);
        Task AddAsync(DangKyHocPhan dangKy);

        Task<DangKyHocPhan?> GetDangKyAsync(
            string maSV, string maLopHocPhan);

        Task UpdateAsync(DangKyHocPhan dangKy);
        Task SaveChangesAsync();
    }
}
