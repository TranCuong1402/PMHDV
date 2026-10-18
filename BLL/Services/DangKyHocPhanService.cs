
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class DangKyHocPhanService : IDangKyHocPhanService
    {
        private readonly IDangKyHocPhanRepository _repository;

        public DangKyHocPhanService(
            IDangKyHocPhanRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> DangKyAsync(
            DangKyHocPhanRequest request)
        {
            if (request == null)
                throw new ArgumentException("Dữ liệu đăng ký không hợp lệ.");

            string maSV = request.MaSV?.Trim() ?? string.Empty;
            string maLopHocPhan =
                request.MaLopHocPhan?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(maSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(maLopHocPhan))
                throw new ArgumentException("Mã lớp học phần không được để trống.");

            if (!await _repository.SinhVienExistsAsync(maSV))
                throw new ArgumentException(
                    "Sinh viên không tồn tại hoặc đã bị khóa.");

            if (!await _repository.LopHocPhanExistsAsync(maLopHocPhan))
                throw new ArgumentException(
                    "Lớp học phần không tồn tại hoặc đã đóng.");

            if (await _repository.DaDangKyAsync(maSV, maLopHocPhan))
                throw new ArgumentException(
                    "Sinh viên đã đăng ký lớp học phần này.");

            int? siSoToiDa =
                await _repository.LaySiSoToiDaAsync(maLopHocPhan);

            if (siSoToiDa == null || siSoToiDa <= 0)
                throw new ArgumentException(
                    "Sĩ số tối đa của lớp học phần không hợp lệ.");

            int soLuong =
                await _repository.DemSoLuongDangKyAsync(maLopHocPhan);

            if (soLuong >= siSoToiDa.Value)
                throw new ArgumentException(
                    "Lớp học phần đã đủ sĩ số.");

            var dangKy = new DangKyHocPhan
            {
                MaSV = maSV,
                MaLopHocPhan = maLopHocPhan,
                NgayDangKy = DateTime.Now,
                TrangThai = "Đã đăng ký"
            };


            await _repository.AddAsync(dangKy);
            await _repository.SaveChangesAsync();

            return "Đăng ký học phần thành công.";

        }

        public async Task<string> HuyDangKyAsync(
            HuyDangKyHocPhanRequest request)
        {
            if (request == null)
                throw new ArgumentException("Dữ liệu hủy đăng ký không hợp lệ.");

            string maSV = request.MaSV?.Trim() ?? string.Empty;
            string maLopHocPhan =
                request.MaLopHocPhan?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(maSV) ||
                string.IsNullOrWhiteSpace(maLopHocPhan))
            {
                throw new ArgumentException(
                    "Mã sinh viên và mã lớp học phần không được để trống.");
            }

            var dangKy = await _repository.GetDangKyAsync(
                maSV, maLopHocPhan);

            if (dangKy == null)
                throw new ArgumentException(
                    "Không tìm thấy đăng ký học phần của sinh viên.");

            if (dangKy.TrangThai != "Đã đăng ký")
                throw new ArgumentException(
                    "Đăng ký này đã được hủy hoặc không còn hiệu lực.");

            dangKy.TrangThai = "Đã hủy";

            await _repository.UpdateAsync(dangKy);
            await _repository.SaveChangesAsync();

            return "Hủy đăng ký học phần thành công.";
        }

        public async Task<KiemTraDieuKienResponse> KiemTraDieuKienAsync(
            KiemTraDieuKienRequest request)
        {
            var ketQua = new KiemTraDieuKienResponse();

            if (request == null)
            {
                ketQua.DuDieuKien = false;
                ketQua.ThongBao = "Dữ liệu kiểm tra không hợp lệ.";
                ketQua.LyDo.Add("Vui lòng cung cấp thông tin đăng ký.");
                return ketQua;
            }

            string maSV = request.MaSV?.Trim() ?? string.Empty;
            string maLopHocPhan =
                request.MaLopHocPhan?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(maSV))
                ketQua.LyDo.Add("Mã sinh viên không được để trống.");

            if (string.IsNullOrWhiteSpace(maLopHocPhan))
                ketQua.LyDo.Add("Mã lớp học phần không được để trống.");

            if (ketQua.LyDo.Count > 0)
            {
                ketQua.DuDieuKien = false;
                ketQua.ThongBao = "Thông tin kiểm tra chưa đầy đủ.";
                return ketQua;
            }

            bool sinhVienHopLe =
                await _repository.SinhVienExistsAsync(maSV);

            if (!sinhVienHopLe)
                ketQua.LyDo.Add("Sinh viên không tồn tại hoặc đã bị khóa.");

            bool lopHocPhanHopLe =
                await _repository.LopHocPhanExistsAsync(maLopHocPhan);

            if (!lopHocPhanHopLe)
            {
                ketQua.LyDo.Add(
                    "Lớp học phần không tồn tại hoặc đã đóng.");
            }
            else
            {
                bool daDangKy = await _repository.DaDangKyAsync(
                    maSV, maLopHocPhan);

                if (daDangKy)
                    ketQua.LyDo.Add(
                        "Sinh viên đã đăng ký lớp học phần này.");

                int? siSoToiDa =
                    await _repository.LaySiSoToiDaAsync(maLopHocPhan);

                if (siSoToiDa == null || siSoToiDa <= 0)
                {
                    ketQua.LyDo.Add(
                        "Sĩ số tối đa của lớp học phần không hợp lệ.");
                }
                else
                {
                    int soLuong =
                        await _repository.DemSoLuongDangKyAsync(
                            maLopHocPhan);

                    if (soLuong >= siSoToiDa.Value)
                        ketQua.LyDo.Add("Lớp học phần đã đủ sĩ số.");
                }
            }

            ketQua.DuDieuKien = ketQua.LyDo.Count == 0;

            ketQua.ThongBao = ketQua.DuDieuKien
                ? "Sinh viên đủ điều kiện đăng ký học phần."
                : "Sinh viên chưa đủ điều kiện đăng ký học phần.";

            return ketQua;
        }


    }
}
