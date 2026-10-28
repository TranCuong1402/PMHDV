namespace QlyDaoTao.Model.DTOs
{
    public class NhapDiemRequest
    {
        public string MaSV { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public decimal? DiemChuyenCan { get; set; }

        public decimal? DiemGiuaKy { get; set; }

        public decimal? DiemCuoiKy { get; set; }
    }
}