
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/tylevang")]
    public class TyLeVangController : ControllerBase
    {
        private readonly ITyLeVangService _service;

        public TyLeVangController(ITyLeVangService service)
        {
            _service = service;
        }

        // GET: api/nguoidung/tylevang/SV001
        [HttpGet("{maSV}")]
        public async Task<IActionResult> TinhTyLeVang(string maSV)
        {
            try
            {
                var ketQua = await _service.TinhTyLeVangAsync(maSV);

                return Ok(new
                {
                    success = true,
                    message = "Tính tỷ lệ vắng thành công.",
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
