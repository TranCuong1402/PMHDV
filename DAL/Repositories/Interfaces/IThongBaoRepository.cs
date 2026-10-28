using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public interface IThongBaoRepository
    {
        Task<bool> LichHocExistsAsync(int maLichHoc);

        Task<List<int>> GetTaiKhoanSinhVienAsync(
            string maLopHocPhan);

        Task<int?> GetTaiKhoanGiangVienAsync(
            int maLichHoc);

        Task AddRangeAsync(List<ThongBao> thongBaos);

        Task<string> GetMaLopHocPhanAsync(int maLichHoc);
        Task SaveChangesAsync();
    }
}