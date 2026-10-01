namespace QlyDaoTao.Model.Entities
{
    public class Diem
    {
        public int Id { get; set; }

        public string MaSV { get; set; } = null!;

        public string MaLopHocPhan { get; set; } = null!;

        public decimal? DiemChuyenCan { get; set; }

        public decimal? DiemGiuaKy { get; set; }

        public decimal? DiemCuoiKy { get; set; }

        public decimal? DiemTongKet { get; set; }

        public string? XepLoai { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}