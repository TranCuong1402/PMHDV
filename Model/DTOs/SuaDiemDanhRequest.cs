
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class SuaDiemDanhRequest
    {
        [Required]
        public string TrangThai { get; set; } = string.Empty;

        public string? LyDo { get; set; }
    }
}
