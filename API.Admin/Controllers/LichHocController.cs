
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/lichhoc")]
    public class LichHocController : ControllerBase
    {
        private readonly ILichHocService _service;

        public LichHocController(ILichHocService service)
        {
            _service = service;
        }

        // GET: api/admin/lichhoc
        // GET: api/admin/lichhoc?keyword=MH001
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/lichhoc/1
        [HttpGet("{maLichHoc:int}")]
        public async Task<IActionResult> GetById(int maLichHoc)
        {
            var data = await _service.GetByIdAsync(maLichHoc);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy lịch học."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/lichhoc
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LichHoc lichHoc)
        {
            var result = await _service.AddAsync(lichHoc);

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

        // PUT: api/admin/lichhoc/1
        [HttpPut("{maLichHoc:int}")]
        public async Task<IActionResult> Update(
            int maLichHoc,
            [FromBody] LichHoc lichHoc)
        {
            var result = await _service.UpdateAsync(maLichHoc, lichHoc);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy lịch học.")
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

        // DELETE: api/admin/lichhoc/1
        [HttpDelete("{maLichHoc:int}")]
        public async Task<IActionResult> Delete(int maLichHoc)
        {
            var result = await _service.DeleteAsync(maLichHoc);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy lịch học.")
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
