
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/kiemtratrunglich")]
    public class KiemTraTrungLichController : ControllerBase
    {
        private readonly ILichHocService _service;

        public KiemTraTrungLichController(
            ILichHocService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> KiemTra(
            [FromBody] KiemTraTrungLichRequest request)
        {
            try
            {
                var ketQua = await _service
                    .KiemTraTrungLichAsync(request);

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
        }
    }
}
