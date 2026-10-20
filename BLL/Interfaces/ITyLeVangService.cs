
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface ITyLeVangService
    {
        Task<List<TyLeVangDto>> TinhTyLeVangAsync(string maSV);
    }
}
