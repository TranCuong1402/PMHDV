
namespace QlyDaoTao.Model.DTOs
{
    public class LichGiangDayDto
    {
        public int MaLichHoc { get; set; }

        public string MaGiangVien { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public string MaMonHoc { get; set; } = string.Empty;

        public string TenMonHoc { get; set; } = string.Empty;

        public DateTime NgayHoc { get; set; }

        public TimeSpan GioBatDau { get; set; }

        public TimeSpan GioKetThuc { get; set; }

        public string? MaPhong { get; set; }

        public string TrangThai { get; set; } = string.Empty;
    }
}
