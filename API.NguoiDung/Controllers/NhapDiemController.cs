using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/diem")]
    public class NhapDiemController : ControllerBase
    {
        private readonly IDiemService _service;

        public NhapDiemController(IDiemService service)
        {
            _service = service;
        }

        [HttpPost("nhap")]
        public async Task<IActionResult> NhapDiem(
            [FromBody] NhapDiemRequest request)
        {
            try
            {
                await _service.NhapDiemAsync(request);

                return Ok(new
                {
                    success = true,
                    message = "Nhập điểm thành công."
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}