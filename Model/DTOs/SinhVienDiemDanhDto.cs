namespace QlyDaoTao.Model.DTOs
{
    public class SinhVienDiemDanhDto
    {
        public string MaSV { get; set; } = null!;

        public string? TrangThaiDiemDanh { get; set; }

        public DateTime? ThoiGianDiemDanh { get; set; }

        public string? LyDo { get; set; }

        public bool DaDiemDanh { get; set; }

        public string? TrangThaiDangKy { get; set; }
    }
}