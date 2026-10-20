
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class CanhBaoVangRequest
    {
        [Required]
        public string MaSV { get; set; } = string.Empty;

        [Required]
        public string MaLopHocPhan { get; set; } = string.Empty;

        public decimal NguongCanhBao { get; set; } = 20;
    }
}
