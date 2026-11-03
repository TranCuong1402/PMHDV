
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/suadiem")]
    public class SuaDiemController : ControllerBase
    {
        private readonly IDiemService _service;

        public SuaDiemController(IDiemService service)
        {
            _service = service;
        }

        [HttpPut("{maSV}/{maLopHocPhan}")]
        public async Task<IActionResult> SuaDiem(
            string maSV,
            string maLopHocPhan,
            [FromBody] SuaDiemRequest request)
        {
            try
            {
                await _service.SuaDiemAsync(
                    maSV, maLopHocPhan, request);

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật điểm thành công."
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
