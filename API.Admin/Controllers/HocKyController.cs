
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/hocky")]
    public class HocKyController : ControllerBase
    {
        private readonly IHocKyService _service;

        public HocKyController(IHocKyService service)
        {
            _service = service;
        }

        // GET: api/admin/hocky
        // GET: api/admin/hocky?keyword=HK1
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/hocky/HK2025_1
        [HttpGet("{maHocKy}")]
        public async Task<IActionResult> GetById(string maHocKy)
        {
            var data = await _service.GetByIdAsync(maHocKy);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy học kỳ."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/hocky
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] HocKy hocKy)
        {
            var result = await _service.AddAsync(hocKy);

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

        // PUT: api/admin/hocky/HK2025_1
        [HttpPut("{maHocKy}")]
        public async Task<IActionResult> Update(
            string maHocKy,
            [FromBody] HocKy hocKy)
        {
            var result = await _service.UpdateAsync(
                maHocKy, hocKy);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy học kỳ.")
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

        // DELETE: api/admin/hocky/HK2025_1
        [HttpDelete("{maHocKy}")]
        public async Task<IActionResult> Delete(string maHocKy)
        {
            var result = await _service.DeleteAsync(maHocKy);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy học kỳ.")
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
