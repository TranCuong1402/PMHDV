using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public interface IDiemService
    {
        Task NhapDiemAsync(NhapDiemRequest request);
    }
}