
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/diemdanh")]
    public class DiemDanhController : ControllerBase
    {
        private readonly IDiemDanhService _service;

        public DiemDanhController(IDiemDanhService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> DiemDanh(
            [FromBody] DiemDanhRequest request)
        {
            try
            {
                var message = await _service.DiemDanhAsync(request);
                return Ok(new { message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{maLichHoc:int}/{maSV}")]
        public async Task<IActionResult> SuaDiemDanh(
            int maLichHoc,
            string maSV,
            [FromBody] SuaDiemDanhRequest request)
        {
            try
            {
                var ketQua = await _service.SuaDiemDanhAsync(
                    maLichHoc,
                    maSV,
                    request);

                return Ok(new
                {
                    success = true,
                    message = ketQua
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
