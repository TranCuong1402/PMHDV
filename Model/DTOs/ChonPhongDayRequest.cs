
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class ChonPhongDayRequest
    {
        [Required]
        public string MaGiangVien { get; set; } = string.Empty;

        [Required]
        public string MaPhong { get; set; } = string.Empty;
    }
}
