using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;

namespace QlyDaoTao.API.Admin.Controllers
{
    [ApiController]
    [Route("api/admin/lophocphan")]
    public class LopHocPhanController : ControllerBase
    {
        private readonly ILopHocPhanService _service;

        public LopHocPhanController(ILopHocPhanService service)
        {
            _service = service;
        }

        // GET: api/admin/lophocphan
        // GET: api/admin/lophocphan?keyword=MH001
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? keyword)
        {
            var result = string.IsNullOrWhiteSpace(keyword)
                ? await _service.GetAllAsync()
                : await _service.SearchAsync(keyword);

            return Ok(result);
        }

        // GET: api/admin/lophocphan/LHP001
        [HttpGet("{maLopHocPhan}")]
        public async Task<IActionResult> GetById(string maLopHocPhan)
        {
            var result = await _service.GetByIdAsync(maLopHocPhan);

            if (result == null)
                return NotFound(new
                {
                    message = "Không tìm thấy lớp học phần."
                });

            return Ok(result);
        }
    }
}