using QlyDaoTao.DAL.Repositories;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services
{
    public class PhucKhaoService : IPhucKhaoService
    {
        private readonly IPhucKhaoRepository _repository;

        public PhucKhaoService(IPhucKhaoRepository repository)
        {
            _repository = repository;
        }

        public async Task GuiYeuCauAsync(GuiPhucKhaoRequest request)
        {
            if (request == null)
                throw new ArgumentException("Dữ liệu không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.MaSV))
                throw new ArgumentException("Mã sinh viên không được để trống.");

            if (request.MaDiem <= 0)
                throw new ArgumentException("Mã điểm không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.NoiDung))
                throw new ArgumentException("Nội dung phúc khảo không được để trống.");

            if (request.NoiDung.Trim().Length > 1000)
                throw new ArgumentException("Nội dung không được vượt quá 1000 ký tự.");

            bool sinhVienTonTai =
                await _repository.SinhVienExistsAsync(request.MaSV);

            if (!sinhVienTonTai)
                throw new KeyNotFoundException("Sinh viên không tồn tại.");

            var diem = await _repository.GetDiemAsync(request.MaDiem);

            if (diem == null)
                throw new KeyNotFoundException("Không tìm thấy điểm.");

            if (diem.MaSV != request.MaSV)
                throw new InvalidOperationException(
                    "Điểm này không thuộc về sinh viên.");

            if (diem.DiemTongKet == null)
                throw new InvalidOperationException(
                    "Chưa có điểm tổng kết nên không thể phúc khảo.");

            bool daGui = await _repository.DaGuiYeuCauAsync(
                request.MaSV, request.MaDiem);

            if (daGui)
                throw new InvalidOperationException(
                    "Bạn đã gửi yêu cầu phúc khảo cho điểm này.");

            var phucKhao = new PhucKhao
            {
                MaSV = request.MaSV,
                MaDiem = request.MaDiem,
                NoiDung = request.NoiDung.Trim(),

                DiemCu = diem.DiemTongKet,
                DiemMoi = null,

                TrangThai = "CHO_DUYET",
                LyDoXuLy = null,
                NguoiXuLy = null,

                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            await _repository.AddAsync(phucKhao);
            await _repository.SaveChangesAsync();
        }
        public async Task<List<PhucKhaoResponse>> GetAllAsync()
        {
            var danhSach = await _repository.GetAllAsync();

            return danhSach.Select(x => new PhucKhaoResponse
            {
                Id = x.Id,
                MaSV = x.MaSV,
                MaDiem = x.MaDiem,
                NoiDung = x.NoiDung,
                DiemCu = x.DiemCu,
                DiemMoi = x.DiemMoi,
                TrangThai = x.TrangThai,
                LyDoXuLy = x.LyDoXuLy,
                NguoiXuLy = x.NguoiXuLy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
        }

        public async Task<PhucKhaoResponse> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Mã yêu cầu phúc khảo không hợp lệ.");

            var x = await _repository.GetByIdAsync(id);

            if (x == null)
                throw new KeyNotFoundException("Không tìm thấy yêu cầu phúc khảo.");

            return new PhucKhaoResponse
            {
                Id = x.Id,
                MaSV = x.MaSV,
                MaDiem = x.MaDiem,
                NoiDung = x.NoiDung,
                DiemCu = x.DiemCu,
                DiemMoi = x.DiemMoi,
                TrangThai = x.TrangThai,
                LyDoXuLy = x.LyDoXuLy,
                NguoiXuLy = x.NguoiXuLy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            };
        }
        public async Task DuyetYeuCauAsync(
    int id, DuyetPhucKhaoRequest request)
        {
            if (id <= 0)
                throw new ArgumentException("Mã yêu cầu phúc khảo không hợp lệ.");

            if (request == null ||
                string.IsNullOrWhiteSpace(request.NguoiXuLy))
            {
                throw new ArgumentException(
                    "Mã người xử lý không được để trống.");
            }
            bool nguoiXuLyTonTai =
    await _repository.NguoiXuLyExistsAsync(
        request.NguoiXuLy.Trim());

            if (!nguoiXuLyTonTai)
            {
                throw new KeyNotFoundException(
                    "Mã giảng viên xử lý không tồn tại.");
            }

            var phucKhao = await _repository.GetByIdAsync(id);

            if (phucKhao == null)
                throw new KeyNotFoundException(
                    "Không tìm thấy yêu cầu phúc khảo.");

            if (phucKhao.TrangThai != "CHO_DUYET")
                throw new InvalidOperationException(
                    "Yêu cầu này không ở trạng thái chờ duyệt.");

            phucKhao.TrangThai = "DA_DUYET";
            phucKhao.NguoiXuLy = request.NguoiXuLy.Trim();
            phucKhao.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(phucKhao);
            await _repository.SaveChangesAsync();
        }
        public async Task TuChoiYeuCauAsync(
    int id, TuChoiPhucKhaoRequest request)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "Mã yêu cầu phúc khảo không hợp lệ.");

            if (request == null)
                throw new ArgumentException("Dữ liệu không hợp lệ.");

            if (string.IsNullOrWhiteSpace(request.NguoiXuLy))
                throw new ArgumentException(
                    "Mã người xử lý không được để trống.");

            if (string.IsNullOrWhiteSpace(request.LyDoXuLy))
                throw new ArgumentException(
                    "Lý do từ chối không được để trống.");

            if (request.LyDoXuLy.Trim().Length > 1000)
                throw new ArgumentException(
                    "Lý do từ chối không được vượt quá 1000 ký tự.");

            // Kiểm tra giảng viên xử lý
            bool nguoiXuLyTonTai =
                await _repository.NguoiXuLyExistsAsync(
                    request.NguoiXuLy.Trim());

            if (!nguoiXuLyTonTai)
                throw new KeyNotFoundException(
                    "Mã giảng viên xử lý không tồn tại.");

            // Tìm yêu cầu phúc khảo
            var phucKhao = await _repository.GetByIdAsync(id);

            if (phucKhao == null)
                throw new KeyNotFoundException(
                    "Không tìm thấy yêu cầu phúc khảo.");

            // Chỉ được từ chối yêu cầu đang chờ duyệt
            if (phucKhao.TrangThai != "CHO_DUYET")
                throw new InvalidOperationException(
                    "Yêu cầu này không ở trạng thái chờ duyệt.");

            // Cập nhật thông tin từ chối
            phucKhao.TrangThai = "TU_CHOI";
            phucKhao.LyDoXuLy = request.LyDoXuLy.Trim();
            phucKhao.NguoiXuLy = request.NguoiXuLy.Trim();
            phucKhao.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(phucKhao);
            await _repository.SaveChangesAsync();
        }
        public async Task CapNhatDiemAsync(
    int id, CapNhatDiemPhucKhaoRequest request)
        {
            // 1. Kiểm tra dữ liệu đầu vào
            if (id <= 0)
                throw new ArgumentException(
                    "Mã yêu cầu phúc khảo không hợp lệ.");

            if (request == null)
                throw new ArgumentException(
                    "Dữ liệu cập nhật không hợp lệ.");

            if (request.DiemMoi < 0 || request.DiemMoi > 10)
                throw new ArgumentException(
                    "Điểm mới phải nằm trong khoảng từ 0 đến 10.");

            if (string.IsNullOrWhiteSpace(request.NguoiXuLy))
                throw new ArgumentException(
                    "Mã người xử lý không được để trống.");

            // 2. Kiểm tra người xử lý
            bool nguoiXuLyTonTai =
                await _repository.NguoiXuLyExistsAsync(
                    request.NguoiXuLy.Trim());

            if (!nguoiXuLyTonTai)
                throw new KeyNotFoundException(
                    "Mã giảng viên xử lý không tồn tại.");

            // 3. Tìm yêu cầu phúc khảo
            var phucKhao = await _repository.GetByIdAsync(id);

            if (phucKhao == null)
                throw new KeyNotFoundException(
                    "Không tìm thấy yêu cầu phúc khảo.");

            // 4. Chỉ cập nhật yêu cầu đã được duyệt
            if (phucKhao.TrangThai != "DA_DUYET")
                throw new InvalidOperationException(
                    "Chỉ được cập nhật điểm khi yêu cầu đã được duyệt.");

            // 5. Lấy thông tin điểm
            var diem = await _repository.GetDiemAsync(
                phucKhao.MaDiem);

            if (diem == null)
                throw new KeyNotFoundException(
                    "Không tìm thấy bản ghi điểm.");

            if (diem.MaSV != phucKhao.MaSV)
                throw new InvalidOperationException(
                    "Thông tin sinh viên và bản ghi điểm không khớp.");

            // 6. Cập nhật điểm chính thức
            diem.DiemTongKet = request.DiemMoi;
            diem.UpdatedAt = DateTime.Now;

            // 7. Cập nhật thông tin phúc khảo
            phucKhao.DiemMoi = request.DiemMoi;
            phucKhao.TrangThai = "DA_CAP_NHAT";
            phucKhao.NguoiXuLy = request.NguoiXuLy.Trim();
            phucKhao.UpdatedAt = DateTime.Now;

            // 8. Lưu cả hai thay đổi
            await _repository.UpdateDiemAsync(diem);
            await _repository.UpdateAsync(phucKhao);

            await _repository.SaveChangesAsync();
        }
    }
}