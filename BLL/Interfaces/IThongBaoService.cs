using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public interface IThongBaoService
    {
        Task<int> ThongBaoThayDoiLichAsync(
            ThongBaoThayDoiLichRequest request);
    }
}