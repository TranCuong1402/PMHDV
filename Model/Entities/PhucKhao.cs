namespace QlyDaoTao.Model.Entities
{
    public class PhucKhao
    {
        public int Id { get; set; }

        public string MaSV { get; set; } = null!;

        public int MaDiem { get; set; }

        public string NoiDung { get; set; } = null!;

        public decimal? DiemCu { get; set; }

        public decimal? DiemMoi { get; set; }

        public string TrangThai { get; set; } = null!;

        public string? LyDoXuLy { get; set; }

        public string? NguoiXuLy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}