using QlyDaoTao.Model.DTOs;

public interface ITaiKhoanService
{
    Task<DangNhapResponse> DangNhapAsync(DangNhapRequest request);
    Task<string> DangKyAsync(DangKyRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<string> QuenMatKhauAsync(QuenMatKhauRequest request);
    Task<string> XacThucOtpAsync(XacThucOtpRequest request);
}