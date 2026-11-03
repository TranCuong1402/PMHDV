using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.NguoiDung.Controllers
{
    [Route("api/nguoidung/phuckhao")]
    [ApiController]
    public class PhucKhaoController : ControllerBase
    {
        private readonly IPhucKhaoService _service;

        public PhucKhaoController(IPhucKhaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> GuiYeuCau(
            [FromBody] GuiPhucKhaoRequest request)
        {
            try
            {
                await _service.GuiYeuCauAsync(request);

                return Ok(new
                {
                    message = "Gửi yêu cầu phúc khảo thành công."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // UC36: Xem danh sách yêu cầu phúc khảo
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var danhSach = await _service.GetAllAsync();

                return Ok(danhSach);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy danh sách phúc khảo.",
                    error = ex.Message
                });
            }
        }

        // UC36: Xem chi tiết yêu cầu phúc khảo
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var phucKhao = await _service.GetByIdAsync(id);

                return Ok(phucKhao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy chi tiết phúc khảo.",
                    error = ex.Message
                });
            }
        }
        // UC37: Phê duyệt yêu cầu phúc khảo
        [HttpPut("{id:int}/duyet")]
        public async Task<IActionResult> DuyetYeuCau(
            int id,
            [FromBody] DuyetPhucKhaoRequest request)
        {
            try
            {
                await _service.DuyetYeuCauAsync(id, request);

                return Ok(new
                {
                    message = "Phê duyệt yêu cầu phúc khảo thành công."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // UC38: Từ chối yêu cầu phúc khảo
        [HttpPut("{id:int}/tuchoi")]
        public async Task<IActionResult> TuChoiYeuCau(
            int id,
            [FromBody] TuChoiPhucKhaoRequest request)
        {
            try
            {
                await _service.TuChoiYeuCauAsync(id, request);

                return Ok(new
                {
                    message = "Từ chối yêu cầu phúc khảo thành công."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        // UC39: Cập nhật điểm sau phúc khảo
        [HttpPut("{id:int}/capnhatdiem")]
        public async Task<IActionResult> CapNhatDiem(
            int id,
            [FromBody] CapNhatDiemPhucKhaoRequest request)
        {
            try
            {
                await _service.CapNhatDiemAsync(id, request);

                return Ok(new
                {
                    message = "Cập nhật điểm sau phúc khảo thành công."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}