namespace QlyDaoTao.Model.DTOs
{
    public class DangNhapResponse
    {
        public int MaTaiKhoan { get; set; }
        public string TenDangNhap { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;
        public string Token { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

    }
}