
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;
using System.Security.Claims;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/diemdanh")]
    [Authorize(Roles = "Admin,GiangVien")]
    public class DiemDanhController : ControllerBase
    {
        private readonly IDiemDanhService _service;
        private readonly IDiemDanhService _diemDanhService;
        public DiemDanhController(IDiemDanhService service, IDiemDanhService diemDanhService)
        {
            _service = service;
            _diemDanhService = diemDanhService;
        } 
      

        private int GetTaiKhoanId()
        {
            var id = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(id, out int idTaiKhoan))
                throw new UnauthorizedAccessException();

            return idTaiKhoan;
        }

        [HttpPost]
        public async Task<IActionResult> DiemDanh(
            [FromBody] DiemDanhRequest request)
        {
            try
            {
                int idTaiKhoan = GetTaiKhoanId();
                string vaiTro = User.FindFirstValue(
                    ClaimTypes.Role) ?? "";

                var message = await _service.DiemDanhAsync(
                    request,
                    idTaiKhoan,
                    vaiTro);

                return Ok(new
                {
                    success = true,
                    message = message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("{maLichHoc:int}/{maSV}")]
        public async Task<IActionResult> SuaDiemDanh(
            int maLichHoc,
            string maSV,
            [FromBody] SuaDiemDanhRequest request)
        {
            try
            {
                int idTaiKhoan = GetTaiKhoanId();
                string vaiTro = User.FindFirstValue(
                    ClaimTypes.Role) ?? "";

                var ketQua = await _service.SuaDiemDanhAsync(
                    maLichHoc,
                    maSV,
                    request,
                    idTaiKhoan,
                    vaiTro);

                return Ok(new
                {
                    success = true,
                    message = ketQua
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                int idTaiKhoan = GetTaiKhoanId();

                string vaiTro = User.FindFirstValue(
                    ClaimTypes.Role) ?? "";

                var ketQua = await _service.GetAllAsync(
                    idTaiKhoan,
                    vaiTro);

                return Ok(new
                {
                    success = true,
                    data = ketQua
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("lichhoc/{maLichHoc:int}")]
        public async Task<IActionResult> GetByLichHoc(
            int maLichHoc)
        {
            try
            {
                int idTaiKhoan = GetTaiKhoanId();

                string vaiTro = User.FindFirstValue(
                    ClaimTypes.Role) ?? "";

                var ketQua = await _service.GetByLichHocAsync(
                    maLichHoc,
                    idTaiKhoan,
                    vaiTro);

                return Ok(new
                {
                    success = true,
                    data = ketQua
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }
        [HttpDelete("{maLichHoc:int}/{maSV}")]
        public async Task<IActionResult> XoaDiemDanh(
    int maLichHoc,
    string maSV)
        {
            try
            {
                int idTaiKhoan = GetTaiKhoanId();

                string vaiTro = User.FindFirstValue(
                    ClaimTypes.Role) ?? "";

                var ketQua = await _service.XoaDiemDanhAsync(
                    maLichHoc,
                    maSV,
                    idTaiKhoan,
                    vaiTro);

                return Ok(new
                {
                    success = true,
                    message = ketQua
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("lichhoc/{maLichHoc}/danhsach-sinhvien")]
        public async Task<IActionResult> LayDanhSachSinhVienTheoLichHoc(
    int maLichHoc)
        {
            try
            {
                int idTaiKhoan = int.Parse(
                    User.FindFirstValue(ClaimTypes.NameIdentifier)!
                );

                string vaiTro = User.FindFirstValue(ClaimTypes.Role)!;

                var result = await _diemDanhService
                    .LayDanhSachSinhVienTheoLichHocAsync(
                        maLichHoc,
                        idTaiKhoan,
                        vaiTro
                    );

                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách sinh viên thành công.",
                    data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
