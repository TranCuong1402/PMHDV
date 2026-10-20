
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/canhbaovang")]
    public class CanhBaoVangController : ControllerBase
    {
        private readonly ICanhBaoVangService _service;

        public CanhBaoVangController(ICanhBaoVangService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> TaoCanhBao(
            [FromBody] CanhBaoVangRequest request)
        {
            try
            {
                var ketQua = await _service.TaoCanhBaoAsync(request);

                return Ok(new
                {
                    success = true,
                    message = "Tạo hoặc cập nhật cảnh báo thành công.",
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

        [HttpGet("{maSV}")]
        public async Task<IActionResult> GetBySinhVien(string maSV)
        {
            try
            {
                var ketQua = await _service.GetBySinhVienAsync(maSV);

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
