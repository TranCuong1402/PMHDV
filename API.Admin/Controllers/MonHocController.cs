using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/monhoc")]
    public class MonHocController : ControllerBase
    {
        private readonly IMonHocService _service;

        public MonHocController(IMonHocService service)
        {
            _service = service;
        }

        // GET: api/admin/monhoc
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/monhoc/MH001
        [HttpGet("{maMonHoc}")]
        public async Task<IActionResult> GetById(string maMonHoc)
        {
            var data = await _service.GetByIdAsync(maMonHoc);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy môn học."
                });
            }

            return Ok(data);
        }

        // POST: api/admin/monhoc
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] MonHoc monHoc)
        {
            var result = await _service.AddAsync(monHoc);

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

        // PUT: api/admin/monhoc/MH001
        [HttpPut("{maMonHoc}")]
        public async Task<IActionResult> Update(
            string maMonHoc,
            [FromBody] MonHoc monHoc)
        {
            var result = await _service.UpdateAsync(
                maMonHoc,
                monHoc);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy môn học.")
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

        // DELETE: api/admin/monhoc/MH001
        [HttpDelete("{maMonHoc}")]
        public async Task<IActionResult> Delete(string maMonHoc)
        {
            var result = await _service.DeleteAsync(maMonHoc);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy môn học.")
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