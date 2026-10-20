
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [ApiController]
    [Route("api/nguoidung/sinhvien")]
    public class LopHocPhanSinhVienController : ControllerBase
    {
        private readonly ILopHocPhanSinhVienService _service;

        public LopHocPhanSinhVienController(
            ILopHocPhanSinhVienService service)
        {
            _service = service;
        }

        // GET: api/nguoidung/sinhvien/SV001/lophocphan
        [HttpGet("{maSV}/lophocphan")]
        public async Task<IActionResult> GetBySinhVien(string maSV)
        {
            try
            {
                var result =
                    await _service.GetBySinhVienAsync(maSV);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
