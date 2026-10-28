
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/lichday")]
    public class SuaLichDayController : ControllerBase
    {
        private readonly ILichHocService _service;

        public SuaLichDayController(ILichHocService service)
        {
            _service = service;
        }

        // PUT: api/nguoidung/lichday/1
        [HttpPut("{maLichHoc:int}")]
        public async Task<IActionResult> SuaLichDay(
            int maLichHoc,
            [FromBody] SuaLichDayRequest request)
        {
            try
            {
                await _service.SuaLichDayAsync(maLichHoc, request);

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật lịch dạy thành công."
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
