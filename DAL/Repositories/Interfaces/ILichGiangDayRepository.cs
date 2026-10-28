
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.DAL.Repositories.Interfaces
{
    public interface ILichGiangDayRepository
    {
        Task<bool> GiangVienExistsAsync(string maGiangVien);

        Task<List<LichGiangDayDto>> GetByGiangVienAsync(
            string maGiangVien);
    }
}
