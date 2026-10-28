using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class ThongBaoService : IThongBaoService
    {
        private readonly IThongBaoRepository _repository;

        public ThongBaoService(IThongBaoRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> ThongBaoThayDoiLichAsync(
            ThongBaoThayDoiLichRequest request)
        {
            if (request.MaLichHoc <= 0)
                throw new ArgumentException("Mã lịch học không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.NoiDung))
                throw new ArgumentException("Nội dung thông báo không được để trống.");

            if (!await _repository.LichHocExistsAsync(request.MaLichHoc))
                throw new KeyNotFoundException("Không tìm thấy lịch học.");

            // Lấy thông tin lớp học phần của lịch học.
            // Phần này được truy vấn trực tiếp qua repository ở bước dưới.
            var maLopHocPhan = await GetMaLopHocPhanAsync(
                request.MaLichHoc);

            var taiKhoanSinhVien =
                await _repository.GetTaiKhoanSinhVienAsync(maLopHocPhan);

            var idGiangVien =
                await _repository.GetTaiKhoanGiangVienAsync(
                    request.MaLichHoc);

            if (idGiangVien.HasValue)
                taiKhoanSinhVien.Add(idGiangVien.Value);

            var danhSachId = taiKhoanSinhVien.Distinct().ToList();

            if (danhSachId.Count == 0)
                return 0;

            var thongBaos = danhSachId.Select(userId => new ThongBao
            {
                UserId = userId,
                TieuDe = string.IsNullOrWhiteSpace(request.TieuDe)
                    ? "Thông báo thay đổi lịch học"
                    : request.TieuDe.Trim(),
                NoiDung = request.NoiDung.Trim(),
                LoaiThongBao = "ThayDoiLich",
                DaDoc = false,
                CreatedAt = DateTime.Now
            }).ToList();

            await _repository.AddRangeAsync(thongBaos);
            await _repository.SaveChangesAsync();

            return thongBaos.Count;
        }

        private async Task<string> GetMaLopHocPhanAsync(int maLichHoc)
        {
            return await _repository.GetMaLopHocPhanAsync(maLichHoc);
        }
    }
}