namespace QlyDaoTao.Model.DTOs
{
    public class XacThucOtpRequest
    {
        public string Email { get; set; } = null!;
        public string Otp { get; set; } = null!;
        public string MatKhauMoi { get; set; } = null!;
    }
}