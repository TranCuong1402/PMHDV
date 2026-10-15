
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/nienkhoa")]
    public class NienKhoaController : ControllerBase
    {
        private readonly INienKhoaService _service;

        public NienKhoaController(INienKhoaService service)
        {
            _service = service;
        }

        // GET: api/admin/nienkhoa
        // GET: api/admin/nienkhoa?keyword=2025
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/nienkhoa/NK2025
        [HttpGet("{maNienKhoa}")]
        public async Task<IActionResult> GetById(string maNienKhoa)
        {
            var data = await _service.GetByIdAsync(maNienKhoa);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy niên khóa."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/nienkhoa
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] NienKhoa nienKhoa)
        {
            var result = await _service.AddAsync(nienKhoa);

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

        // PUT: api/admin/nienkhoa/NK2025
        [HttpPut("{maNienKhoa}")]
        public async Task<IActionResult> Update(
            string maNienKhoa,
            [FromBody] NienKhoa nienKhoa)
        {
            var result = await _service.UpdateAsync(
                maNienKhoa, nienKhoa);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy niên khóa.")
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

        // DELETE: api/admin/nienkhoa/NK2025
        [HttpDelete("{maNienKhoa}")]
        public async Task<IActionResult> Delete(string maNienKhoa)
        {
            var result = await _service.DeleteAsync(maNienKhoa);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy niên khóa.")
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
