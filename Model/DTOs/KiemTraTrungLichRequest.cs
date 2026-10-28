
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class KiemTraTrungLichRequest
    {
        [Required]
        public string MaLopHocPhan { get; set; } = string.Empty;

        [Required]
        public DateTime NgayHoc { get; set; }

        [Required]
        public TimeSpan GioBatDau { get; set; }

        [Required]
        public TimeSpan GioKetThuc { get; set; }

        public string? MaPhong { get; set; }

        // Có giá trị khi kiểm tra cập nhật một lịch đã tồn tại.
        public int? MaLichHoc { get; set; }
    }
}
