namespace QlyDaoTao.Model.DTOs
{
    public class PhucKhaoResponse
    {
        public int Id { get; set; }

        public string MaSV { get; set; } = string.Empty;

        public int MaDiem { get; set; }

        public string MaLopHocPhan { get; set; } = string.Empty;

        public string NoiDung { get; set; } = string.Empty;

        public decimal? DiemCu { get; set; }

        public decimal? DiemMoi { get; set; }

        public string TrangThai { get; set; } = string.Empty;

        public string? LyDoXuLy { get; set; }

        public string? NguoiXuLy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}