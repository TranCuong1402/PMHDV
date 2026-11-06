
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public DiemDanhController(IDiemDanhService service)
        {
            _service = service;
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
    }
}
