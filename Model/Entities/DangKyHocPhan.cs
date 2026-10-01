namespace QlyDaoTao.Model.Entities
{
    public class DangKyHocPhan
    {
        public int Id { get; set; }

        public string MaSV { get; set; } = null!;

        public string MaLopHocPhan { get; set; } = null!;

        public DateTime NgayDangKy { get; set; }

        public string TrangThai { get; set; } = null!;
    }
}