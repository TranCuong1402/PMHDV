
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface ILichGiangDayService
    {
        Task<List<LichGiangDayDto>> GetByGiangVienAsync(
            string maGiangVien);
    }
}
