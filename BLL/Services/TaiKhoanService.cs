using QlyDaoTao.BLL.Services;
using QlyDaoTao.BLL.Services.Interfaces;
using QlyDaoTao.DAL.Repositories.Interfaces;
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

public class TaiKhoanService : ITaiKhoanService
{
    private readonly ITaiKhoanRepository _repository;
    private readonly JwtService _jwtService;
    private readonly EmailService _emailService;
    public TaiKhoanService(
        ITaiKhoanRepository repository,
        JwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
    }
    public TaiKhoanService(
    ITaiKhoanRepository repository,
    JwtService jwtService,
    EmailService emailService)
    {
        _repository = repository;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<DangNhapResponse> DangNhapAsync(DangNhapRequest request)
    {
        var taiKhoan = await _repository.GetByTenDangNhapAsync(request.TenDangNhap);

        if (taiKhoan == null || taiKhoan.MatKhauHash != request.MatKhau)
            throw new Exception("Sai tên đăng nhập hoặc mật khẩu!");

        if (!taiKhoan.TrangThai)
            throw new Exception("Tài khoản đã bị khóa!");

        var token = _jwtService.TaoToken(
            taiKhoan.Id,
            taiKhoan.TenDangNhap,
            taiKhoan.VaiTro
        );
        var refreshToken = _jwtService.GenerateRefreshToken();

        taiKhoan.RefreshToken = refreshToken;
        taiKhoan.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _repository.UpdateAsync(taiKhoan);

        return new DangNhapResponse
        {
            MaTaiKhoan = taiKhoan.Id,
            TenDangNhap = taiKhoan.TenDangNhap,
            HoTen = taiKhoan.HoTen,
            VaiTro = taiKhoan.VaiTro,
            Token = token,
            RefreshToken = refreshToken
        };
    }
    public async Task<string> DangKyAsync(DangKyRequest request)
    {
        if (await _repository.ExistsByTenDangNhapAsync(request.TenDangNhap))
        {
            throw new Exception("Tên đăng nhập đã tồn tại!");
        }

        if (string.IsNullOrWhiteSpace(request.TenDangNhap) ||
            string.IsNullOrWhiteSpace(request.MatKhau) ||
            string.IsNullOrWhiteSpace(request.HoTen))
        {
            throw new Exception("Vui lòng nhập đầy đủ thông tin!");
        }

        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = request.TenDangNhap,
            MatKhauHash = request.MatKhau,
            HoTen = request.HoTen,
            Email = request.Email,
            VaiTro = "SinhVien",
            TrangThai = true,
            CreatedAt = DateTime.Now
        };

        await _repository.AddAsync(taiKhoan);

        return "Đăng ký tài khoản thành công!";
    }
    public async Task<RefreshTokenResponse> RefreshTokenAsync(
    RefreshTokenRequest request)
    {
        var taiKhoan = await _repository
            .GetByRefreshTokenAsync(request.RefreshToken);

        if (taiKhoan == null ||
            taiKhoan.RefreshTokenExpiryTime == null ||
            taiKhoan.RefreshTokenExpiryTime <= DateTime.Now ||
            !taiKhoan.TrangThai)
        {
            throw new Exception("Refresh Token không hợp lệ hoặc đã hết hạn!");
        }

        var newAccessToken = _jwtService.GenerateToken(taiKhoan);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        taiKhoan.RefreshToken = newRefreshToken;
        taiKhoan.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

        await _repository.UpdateAsync(taiKhoan);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    public async Task<string> QuenMatKhauAsync(
     QuenMatKhauRequest request)
    {
        var taiKhoan = await _repository.GetByEmailAsync(request.Email);

        if (taiKhoan == null)
            throw new Exception("Email không tồn tại!");

        var otp = Random.Shared.Next(100000, 999999).ToString();

        taiKhoan.MaOtp = otp;
        taiKhoan.OtpHetHan = DateTime.Now.AddMinutes(5);

        await _repository.UpdateAsync(taiKhoan);

        await _emailService.GuiOtpAsync(request.Email, otp);

        return "Mã OTP đã được gửi đến email!";
    }
    public async Task<string> XacThucOtpAsync(XacThucOtpRequest request)
    {
        var taiKhoan = await _repository.GetByEmailAsync(request.Email);

        if (taiKhoan == null)
            throw new Exception("Email không tồn tại!");

        if (string.IsNullOrWhiteSpace(taiKhoan.MaOtp) ||
            taiKhoan.MaOtp != request.Otp)
            throw new Exception("Mã OTP không chính xác!");

        if (taiKhoan.OtpHetHan == null ||
            taiKhoan.OtpHetHan <= DateTime.Now)
            throw new Exception("Mã OTP đã hết hạn!");

        if (string.IsNullOrWhiteSpace(request.MatKhauMoi))
            throw new Exception("Vui lòng nhập mật khẩu mới!");

        taiKhoan.MatKhauHash = request.MatKhauMoi;
        taiKhoan.MaOtp = null;
        taiKhoan.OtpHetHan = null;

        await _repository.UpdateAsync(taiKhoan);

        return "Đặt lại mật khẩu thành công!";
    }
}