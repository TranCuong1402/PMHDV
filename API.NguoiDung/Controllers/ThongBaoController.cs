using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/thongbao")]
    public class ThongBaoController : ControllerBase
    {
        private readonly IThongBaoService _service;

        public ThongBaoController(IThongBaoService service)
        {
            _service = service;
        }

        [HttpPost("thay-doi-lich")]
        public async Task<IActionResult> ThongBaoThayDoiLich(
            [FromBody] ThongBaoThayDoiLichRequest request)
        {
            try
            {
                int soLuong = await _service
                    .ThongBaoThayDoiLichAsync(request);

                return Ok(new
                {
                    success = true,
                    message = "Đã tạo thông báo thay đổi lịch học.",
                    soLuongThongBao = soLuong
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
        }
    }
}