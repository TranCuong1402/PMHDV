using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Interfaces;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/sinhvien")]
    public class SinhVienController : ControllerBase
    {
        private readonly ISinhVienService _service;

        public SinhVienController(ISinhVienService service)
        {
            _service = service;
        }

        // GET: api/admin/sinhvien
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var data = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(data);
        }

        // GET: api/admin/sinhvien/SV001
        [HttpGet("{maSV}")]
        public async Task<IActionResult> GetById(string maSV)
        {
            var data = await _service.GetByIdAsync(maSV);

            if (data == null)
                return NotFound(new
                {
                    message = "Không tìm thấy sinh viên."
                });

            return Ok(data);
        }

        // POST: api/admin/sinhvien
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SinhVien sinhVien)
        {
            var result = await _service.AddAsync(sinhVien);

            if (!result.Success)
                return BadRequest(new
                {
                    message = result.Message
                });

            return Ok(new
            {
                message = result.Message
            });
        }

        // PUT: api/admin/sinhvien/SV001
        [HttpPut("{maSV}")]
        public async Task<IActionResult> Update(
            string maSV,
            [FromBody] SinhVien sinhVien)
        {
            var result = await _service.UpdateAsync(maSV, sinhVien);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy sinh viên.")
                    return NotFound(new
                    {
                        message = result.Message
                    });

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

        // DELETE: api/admin/sinhvien/SV001
        [HttpDelete("{maSV}")]
        public async Task<IActionResult> Delete(string maSV)
        {
            var result = await _service.DeleteAsync(maSV);

            if (!result.Success)
            {
                if (result.Message == "Không tìm thấy sinh viên.")
                    return NotFound(new
                    {
                        message = result.Message
                    });

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