
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public class LichGiangDayService : ILichGiangDayService
    {
        private readonly ILichGiangDayRepository _repository;

        public LichGiangDayService(
            ILichGiangDayRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<LichGiangDayDto>> GetByGiangVienAsync(
            string maGiangVien)
        {
            if (string.IsNullOrWhiteSpace(maGiangVien))
            {
                throw new ArgumentException(
                    "Mã giảng viên không được để trống.");
            }

            maGiangVien = maGiangVien.Trim();

            bool tonTai = await _repository
                .GiangVienExistsAsync(maGiangVien);

            if (!tonTai)
            {
                throw new ArgumentException(
                    "Giảng viên không tồn tại.");
            }

            return await _repository.GetByGiangVienAsync(maGiangVien);
        }
    }
}
