
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class DiemDanhRequest
    {
        [Required]
        public int MaLichHoc { get; set; }

        [Required]
        public string MaSV { get; set; } = string.Empty;

        [Required]
        public string TrangThai { get; set; } = string.Empty;

        public string? LyDo { get; set; }

        public string? NguoiDiemDanh { get; set; }
    }
}
