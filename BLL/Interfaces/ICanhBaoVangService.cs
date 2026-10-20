
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface ICanhBaoVangService
    {
        Task<CanhBaoVangDto> TaoCanhBaoAsync(
            CanhBaoVangRequest request);

        Task<List<CanhBaoVangDto>> GetBySinhVienAsync(string maSV);
    }
}
