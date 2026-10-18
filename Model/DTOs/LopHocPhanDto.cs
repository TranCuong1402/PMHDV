namespace QlyDaoTao.Model.DTOs
{
    public class LopHocPhanDto
    {
        public string MaLopHocPhan { get; set; } = string.Empty;
        public string MaMonHoc { get; set; } = string.Empty;
        public string TenMonHoc { get; set; } = string.Empty;
        public string MaHocKy { get; set; } = string.Empty;
        public string TenHocKy { get; set; } = string.Empty;
        public string MaGiangVien { get; set; } = string.Empty;
        public string TenGiangVien { get; set; } = string.Empty;
        public int SiSoToiDa { get; set; }
        public bool TrangThai { get; set; }
    }
}