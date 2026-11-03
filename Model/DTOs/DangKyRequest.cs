namespace QlyDaoTao.Model.DTOs
{
    public class DangKyRequest
    {
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string? Email { get; set; }
    }
}