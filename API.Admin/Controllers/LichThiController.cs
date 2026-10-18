
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/lichthi")]
    public class LichThiController : ControllerBase
    {
        private readonly ILichThiService _service;

        public LichThiController(ILichThiService service)
        {
            _service = service;
        }

        // GET: api/admin/lichthi
        // GET: api/admin/lichthi?keyword=MH001
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/lichthi/1
        [HttpGet("{maLichThi:int}")]
        public async Task<IActionResult> GetById(int maLichThi)
        {
            var data = await _service.GetByIdAsync(maLichThi);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy lịch thi."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/lichthi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LichThi lichThi)
        {
            var result = await _service.AddAsync(lichThi);

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

        // PUT: api/admin/lichthi/1
        [HttpPut("{maLichThi:int}")]
        public async Task<IActionResult> Update(
            int maLichThi,
            [FromBody] LichThi lichThi)
        {
            var result = await _service.UpdateAsync(maLichThi, lichThi);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy lịch thi.")
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

        // DELETE: api/admin/lichthi/1
        [HttpDelete("{maLichThi:int}")]
        public async Task<IActionResult> Delete(int maLichThi)
        {
            var result = await _service.DeleteAsync(maLichThi);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy lịch thi.")
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

