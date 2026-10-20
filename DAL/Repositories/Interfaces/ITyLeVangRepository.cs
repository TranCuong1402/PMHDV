
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ITyLeVangRepository
    {
        Task<bool> SinhVienExistsAsync(string maSV);

        Task<List<TyLeVangDto>> TinhTyLeVangAsync(string maSV);
    }
}
