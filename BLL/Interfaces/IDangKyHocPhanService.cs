
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface IDangKyHocPhanService
    {
        Task<string> DangKyAsync(DangKyHocPhanRequest request);
        Task<string> HuyDangKyAsync(HuyDangKyHocPhanRequest request);
        Task<KiemTraDieuKienResponse> KiemTraDieuKienAsync(KiemTraDieuKienRequest request);

    }
}
