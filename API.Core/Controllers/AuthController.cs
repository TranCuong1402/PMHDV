using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.API.Core.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITaiKhoanService _service;

        public AuthController(ITaiKhoanService service)
        {
            _service = service;
        }

        // UC01 - Đăng nhập
        [HttpPost("login")]
        public async Task<IActionResult> DangNhap(DangNhapRequest request)
        {
            try
            {
                var result = await _service.DangNhapAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // UC02 - Đăng xuất
        [Authorize]
        [HttpPost("logout")]
        public IActionResult DangXuat()
        {
            return Ok(new
            {
                message = "Đăng xuất thành công!"
            });
        }

        // UC03 - Đăng ký
        [HttpPost("register")]
        public async Task<IActionResult> DangKy(DangKyRequest request)
        {
            try
            {
                var result = await _service.DangKyAsync(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
    RefreshTokenRequest request)
        {
            try
            {
                var result = await _service.RefreshTokenAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> QuenMatKhau(
    QuenMatKhauRequest request)
        {
            try
            {
                var result = await _service.QuenMatKhauAsync(request);

                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> XacThucOtp(
    [FromBody] XacThucOtpRequest request)
        {
            try
            {
                var result = await _service.XacThucOtpAsync(request);
                return Ok(new { message = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}