
using System.ComponentModel.DataAnnotations;

namespace QlyDaoTao.Model.DTOs
{
    public class DangKyHocPhanRequest
    {
        [Required]
        public string MaSV { get; set; } = string.Empty;

        [Required]
        public string MaLopHocPhan { get; set; } = string.Empty;
    }
}
