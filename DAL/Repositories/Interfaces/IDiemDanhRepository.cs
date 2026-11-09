
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface IDiemDanhRepository
    {
        Task<bool> LichHocExistsAsync(int maLichHoc);
        Task<bool> SinhVienExistsAsync(string maSV);
        Task<DiemDanh?> GetByLichHocAndSinhVienAsync(
            int maLichHoc, string maSV);
        Task<List<DiemDanh>> GetAllAsync();
        Task<List<DiemDanh>> GetByGiangVienAsync(
    int idTaiKhoan);
        Task<List<DiemDanh>> GetByLichHocAsync(
            int maLichHoc);
        Task AddAsync(DiemDanh diemDanh);
        Task UpdateAsync(DiemDanh diemDanh);
        Task DeleteAsync(DiemDanh diemDanh);
        Task<List<SinhVienDiemDanhDto>>
    GetDanhSachSinhVienTheoLichHocAsync(int maLichHoc);
        Task<bool> GiangVienPhuTrachLichHocAsync(
         int idTaiKhoan,
         int maLichHoc);
        Task SaveChangesAsync();
    }
}
