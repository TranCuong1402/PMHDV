
namespace QlyDaoTao.Model.DTOs
{
    public class LopHocPhanSinhVienDto
    {
        public string MaSV { get; set; } = string.Empty;
        public string MaLopHocPhan { get; set; } = string.Empty;

        public string MaMonHoc { get; set; } = string.Empty;
        public string TenMonHoc { get; set; } = string.Empty;

        public string MaHocKy { get; set; } = string.Empty;
        public string TenHocKy { get; set; } = string.Empty;

        public string MaGiangVien { get; set; } = string.Empty;
        public string TenGiangVien { get; set; } = string.Empty;

        public int SiSoToiDa { get; set; }
        public DateTime NgayDangKy { get; set; }
        public string TrangThaiDangKy { get; set; } = string.Empty;
    }
}
