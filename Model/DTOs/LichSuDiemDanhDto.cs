
namespace QlyDaoTao.Model.DTOs
{
    public class LichSuDiemDanhDto
    {
        public int Id { get; set; }

        public int MaLichHoc { get; set; }

        public string MaSV { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public DateTime NgayHoc { get; set; }

        public TimeSpan GioBatDau { get; set; }

        public TimeSpan GioKetThuc { get; set; }

        public string? MaPhong { get; set; }

        public string TrangThai { get; set; } = string.Empty;

        public DateTime? ThoiGianDiemDanh { get; set; }

        public string? LyDo { get; set; }

        public string? NguoiDiemDanh { get; set; }
    }
}
