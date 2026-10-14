using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/lop")]
    public class LopController : ControllerBase
    {
        private readonly ILopService _service;

        public LopController(ILopService service)
        {
            _service = service;
        }

        // GET: api/admin/lop
        // GET: api/admin/lop?keyword=CNTT
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/lop/CNTT01
        [HttpGet("{maLop}")]
        public async Task<IActionResult> GetById(string maLop)
        {
            var data = await _service.GetByIdAsync(maLop);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy lớp."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/lop
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Lop lop)
        {
            var result = await _service.AddAsync(lop);

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

        // PUT: api/admin/lop/CNTT02
        [HttpPut("{maLop}")]
        public async Task<IActionResult> Update(
            string maLop,
            [FromBody] Lop lop)
        {
            var result = await _service.UpdateAsync(maLop, lop);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy lớp.")
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

        // DELETE: api/admin/lop/CNTT02
        [HttpDelete("{maLop}")]
        public async Task<IActionResult> Delete(string maLop)
        {
            var result = await _service.DeleteAsync(maLop);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy lớp.")
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