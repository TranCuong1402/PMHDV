namespace QlyDaoTao.Model.Entities
{
    public class Lop
    {
        public string MaLop { get; set; } = null!;

        public string TenLop { get; set; } = null!;

        public string MaNienKhoa { get; set; } = null!;

        public string? Khoa { get; set; }
    }
}