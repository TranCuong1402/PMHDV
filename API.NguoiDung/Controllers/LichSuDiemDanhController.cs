
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/lichsu-diemdanh")]
    public class LichSuDiemDanhController : ControllerBase
    {
        private readonly ILichSuDiemDanhService _service;

        public LichSuDiemDanhController(ILichSuDiemDanhService service)
        {
            _service = service;
        }

        // GET: api/nguoidung/lichsu-diemdanh/SV001
        [HttpGet("{maSV}")]
        public async Task<IActionResult> GetBySinhVien(string maSV)
        {
            try
            {
                var ketQua = await _service.GetBySinhVienAsync(maSV);

                return Ok(new
                {
                    success = true,
                    message = "Lấy lịch sử điểm danh thành công.",
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
