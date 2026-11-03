
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/tinhdiem")]
    public class TinhDiemController : ControllerBase
    {
        private readonly IDiemService _service;

        public TinhDiemController(IDiemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> TinhDiem(
            [FromBody] TinhDiemRequest request)
        {
            try
            {
                var result = await _service.TinhDiemTongKetAsync(
                    request.MaSV,
                    request.MaLopHocPhan);

                return Ok(new
                {
                    success = true,
                    message = "Tính điểm tổng kết thành công.",
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
            catch (InvalidOperationException ex)
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
