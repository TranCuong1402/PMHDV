namespace QlyDaoTao.Model.Entities
{
    public class SinhVien
    {
        public string MaSV { get; set; } = null!;

        public int? IdTaiKhoan { get; set; }

        public string HoTen { get; set; } = null!;

        public DateTime? NgaySinh { get; set; }

        public string? GioiTinh { get; set; }

        public string? Email { get; set; }

        public string? SDT { get; set; }

        public string? DiaChi { get; set; }

        public string MaLop { get; set; } = null!;

        public bool TrangThai { get; set; }
    }
}