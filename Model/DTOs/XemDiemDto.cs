
namespace QlyDaoTao.Model.DTOs
{
    public class XemDiemDto
    {
        public string MaSV { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public string? MaMonHoc { get; set; }

        public string? TenMonHoc { get; set; }

        public string? MaHocKy { get; set; }

        public decimal? DiemChuyenCan { get; set; }

        public decimal? DiemGiuaKy { get; set; }

        public decimal? DiemCuoiKy { get; set; }

        public decimal? DiemTongKet { get; set; }

        public string? XepLoai { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
