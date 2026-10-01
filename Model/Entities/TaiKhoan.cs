namespace QlyDaoTao.Model.Entities
{
    public class TaiKhoan
    {
        public int Id { get; set; }

        public string TenDangNhap { get; set; } = null!;

        public string MatKhauHash { get; set; } = null!;

        public string HoTen { get; set; } = null!;

        public string? Email { get; set; }

        public string VaiTro { get; set; } = null!;

        public bool TrangThai { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}