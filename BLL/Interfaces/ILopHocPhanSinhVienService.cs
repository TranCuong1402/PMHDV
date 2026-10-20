
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface ILopHocPhanSinhVienService
    {
        Task<List<LopHocPhanSinhVienDto>> GetBySinhVienAsync(
            string maSV);
    }
}
