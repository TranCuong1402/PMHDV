namespace QlyDaoTao.Model.Entities
{
    public class GiangVien
    {
        public string MaGiangVien { get; set; } = null!;

        public int? IdTaiKhoan { get; set; }

        public string HoTen { get; set; } = null!;

        public string? Email { get; set; }

        public string? SDT { get; set; }

        public string? Khoa { get; set; }

        public bool TrangThai { get; set; }
    }
}