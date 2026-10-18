
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/dangkyhocphan")]
    public class DangKyHocPhanController : ControllerBase
    {
        private readonly IDangKyHocPhanService _service;

        public DangKyHocPhanController(
            IDangKyHocPhanService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(
            [FromBody] DangKyHocPhanRequest request)
        {
            try
            {
                string message = await _service.DangKyAsync(request);

                return Ok(new { message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("huy")]
        public async Task<IActionResult> HuyDangKy(
            [FromBody] HuyDangKyHocPhanRequest request)
        {
            try
            {
                string message = await _service.HuyDangKyAsync(request);
                return Ok(new { message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("kiem-tra-dieu-kien")]
        public async Task<IActionResult> KiemTraDieuKien(
            [FromBody] KiemTraDieuKienRequest request)
        {
            var ketQua = await _service.KiemTraDieuKienAsync(request);

            return Ok(ketQua);
        }


    }
}
