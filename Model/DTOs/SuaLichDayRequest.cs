
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class SuaLichDayRequest
    {
        [Required]
        public string MaGiangVien { get; set; } = string.Empty;

        [Required]
        public DateTime NgayHoc { get; set; }

        [Required]
        public TimeSpan GioBatDau { get; set; }

        [Required]
        public TimeSpan GioKetThuc { get; set; }

        public string? TrangThai { get; set; }
    }
}
