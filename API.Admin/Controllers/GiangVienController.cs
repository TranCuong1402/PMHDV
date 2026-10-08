using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/giangvien")]
    public class GiangVienController : ControllerBase
    {
        private readonly IGiangVienService _service;

        public GiangVienController(IGiangVienService service)
        {
            _service = service;
        }

        // GET: api/admin/giangvien
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/giangvien/GV001
        [HttpGet("{maGiangVien}")]
        public async Task<IActionResult> GetById(string maGiangVien)
        {
            var data = await _service.GetByIdAsync(maGiangVien);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy giảng viên."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/giangvien
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] GiangVien giangVien)
        {
            var result = await _service.AddAsync(giangVien);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }

        // PUT: api/admin/giangvien/GV001
        [HttpPut("{maGiangVien}")]
        public async Task<IActionResult> Update(
            string maGiangVien,
            [FromBody] GiangVien giangVien)
        {
            var result = await _service.UpdateAsync(
                maGiangVien,
                giangVien);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy giảng viên.")
                {
                    return NotFound(new
                    {
                        message = result.Message
                    });
                }

                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }

        // DELETE: api/admin/giangvien/GV001
        [HttpDelete("{maGiangVien}")]
        public async Task<IActionResult> Delete(string maGiangVien)
        {
            var result = await _service.DeleteAsync(maGiangVien);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy giảng viên.")
                {
                    return NotFound(new
                    {
                        message = result.Message
                    });
                }

                return BadRequest(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }
    }
}