
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/chonphongday")]
    public class ChonPhongDayController : ControllerBase
    {
        private readonly ILichHocService _service;

        public ChonPhongDayController(ILichHocService service)
        {
            _service = service;
        }

        // PUT: api/nguoidung/chonphongday/1
        [HttpPut("{maLichHoc:int}")]
        public async Task<IActionResult> ChonPhongDay(
            int maLichHoc,
            [FromBody] ChonPhongDayRequest request)
        {
            try
            {
                await _service.ChonPhongDayAsync(maLichHoc, request);

                return Ok(new
                {
                    success = true,
                    message = "Chọn phòng dạy thành công."
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
