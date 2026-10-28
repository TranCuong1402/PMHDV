
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/lichgiangday")]
    public class LichGiangDayController : ControllerBase
    {
        private readonly ILichGiangDayService _service;

        public LichGiangDayController(
            ILichGiangDayService service)
        {
            _service = service;
        }

        // GET: api/nguoidung/lichgiangday/GV001
        [HttpGet("{maGiangVien}")]
        public async Task<IActionResult> GetByGiangVien(
            string maGiangVien)
        {
            try
            {
                var ketQua = await _service
                    .GetByGiangVienAsync(maGiangVien);

                return Ok(new
                {
                    success = true,
                    message = "Lấy lịch giảng dạy thành công.",
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
