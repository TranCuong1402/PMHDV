
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class CanhBaoVangService : ICanhBaoVangService
    {
        private readonly ICanhBaoVangRepository _repository;
        private readonly ITyLeVangService _tyLeVangService;

        public CanhBaoVangService(
            ICanhBaoVangRepository repository,
            ITyLeVangService tyLeVangService)
        {
            _repository = repository;
            _tyLeVangService = tyLeVangService;
        }

        public async Task<CanhBaoVangDto> TaoCanhBaoAsync(
            CanhBaoVangRequest request)
        {
            if (request == null)
                throw new ArgumentException("Dữ liệu cảnh báo không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.MaSV) ||
                string.IsNullOrWhiteSpace(request.MaLopHocPhan))
                throw new ArgumentException(
                    "Mã sinh viên và mã lớp học phần không được để trống.");

            if (request.NguongCanhBao < 0 ||
                request.NguongCanhBao > 100)
                throw new ArgumentException(
                    "Ngưỡng cảnh báo phải nằm trong khoảng 0 đến 100.");

            string maSV = request.MaSV.Trim();
            string maLop = request.MaLopHocPhan.Trim();

            if (!await _repository.SinhVienExistsAsync(maSV))
                throw new ArgumentException("Sinh viên không tồn tại.");

            if (!await _repository.LopHocPhanExistsAsync(maLop))
                throw new ArgumentException("Lớp học phần không tồn tại.");

            var danhSach =
                await _tyLeVangService.TinhTyLeVangAsync(maSV);

            var thongKe = danhSach.FirstOrDefault(x =>
                x.MaLopHocPhan == maLop);

            if (thongKe == null || thongKe.TongSoBuoi == 0)
                throw new ArgumentException(
                    "Sinh viên chưa có dữ liệu điểm danh trong lớp học phần này.");

            if (thongKe.TyLeVang <= request.NguongCanhBao)
                throw new ArgumentException(
                    $"Sinh viên chưa vượt ngưỡng cảnh báo. " +
                    $"Tỷ lệ vắng hiện tại: {thongKe.TyLeVang}%.");

            var canhBao = await _repository
                .GetBySinhVienAndLopAsync(maSV, maLop);

            string noiDung =
                $"Sinh viên {maSV} vắng {thongKe.TyLeVang}% " +
                $"số buổi điểm danh, vượt ngưỡng {request.NguongCanhBao}%.";

            if (canhBao == null)
            {
                canhBao = new CanhBaoVang
                {
                    MaSV = maSV,
                    MaLopHocPhan = maLop,
                    TyLeVang = thongKe.TyLeVang,
                    NguongCanhBao = request.NguongCanhBao,
                    NoiDung = noiDung,
                    TrangThai = true,
                    CreatedAt = DateTime.Now
                };

                await _repository.AddAsync(canhBao);
            }
            else
            {
                canhBao.TyLeVang = thongKe.TyLeVang;
                canhBao.NguongCanhBao = request.NguongCanhBao;
                canhBao.NoiDung = noiDung;
                canhBao.TrangThai = true;
            }

            await _repository.SaveChangesAsync();

            return new CanhBaoVangDto
            {
                Id = canhBao.Id,
                MaSV = canhBao.MaSV,
                MaLopHocPhan = canhBao.MaLopHocPhan,
                TyLeVang = canhBao.TyLeVang,
                NguongCanhBao = canhBao.NguongCanhBao,
                NoiDung = canhBao.NoiDung,
                TrangThai = canhBao.TrangThai,
                CreatedAt = canhBao.CreatedAt
            };
        }

        public async Task<List<CanhBaoVangDto>> GetBySinhVienAsync(
    string maSV)
        {
            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException(
                    "Mã sinh viên không được để trống.");

            maSV = maSV.Trim();

            if (!await _repository.SinhVienExistsAsync(maSV))
                throw new ArgumentException("Sinh viên không tồn tại.");

            var danhSach = await _repository.GetBySinhVienAsync(maSV);

            return danhSach.Select(x => new CanhBaoVangDto
            {
                Id = x.Id,
                MaSV = x.MaSV,
                MaLopHocPhan = x.MaLopHocPhan,
                TyLeVang = x.TyLeVang,
                NguongCanhBao = x.NguongCanhBao,
                NoiDung = x.NoiDung,
                TrangThai = x.TrangThai,
                CreatedAt = x.CreatedAt
            }).ToList();
        }

    }
}
