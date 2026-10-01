namespace QlyDaoTao.Model.Entities
{
    public class Phong
    {
        public string MaPhong { get; set; } = null!;

        public string TenPhong { get; set; } = null!;

        public int SucChua { get; set; }

        public string? ViTri { get; set; }

        public bool TrangThai { get; set; }
    }
}