
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/xemdiem")]
    public class XemDiemController : ControllerBase
    {
        private readonly IDiemService _service;

        public XemDiemController(IDiemService service)
        {
            _service = service;
        }

        [HttpGet("{maSV}")]
        public async Task<IActionResult> XemDiem(string maSV)
        {
            try
            {
                var result =
                    await _service.XemDiemSinhVienAsync(maSV);

                return Ok(new
                {
                    success = true,
                    message = "Lấy bảng điểm thành công.",
                    data = result
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
            catch (ArgumentException ex)
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
