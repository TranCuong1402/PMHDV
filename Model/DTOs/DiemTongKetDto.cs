
namespace QlyDaoTao.Model.DTOs
{
    public class DiemTongKetDto
    {
        public string MaSV { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public decimal? DiemChuyenCan { get; set; }

        public decimal? DiemGiuaKy { get; set; }

        public decimal? DiemCuoiKy { get; set; }

        public decimal? DiemTongKet { get; set; }

        public string? XepLoai { get; set; }
    }
}
