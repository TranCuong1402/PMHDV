
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ICanhBaoVangRepository
    {
        Task<bool> SinhVienExistsAsync(string maSV);

        Task<bool> LopHocPhanExistsAsync(string maLopHocPhan);

        Task<CanhBaoVang?> GetBySinhVienAndLopAsync(
            string maSV,
            string maLopHocPhan);

        Task AddAsync(CanhBaoVang canhBao);
        Task<List<CanhBaoVang>> GetBySinhVienAsync(string maSV);

        Task SaveChangesAsync();
    }
}
