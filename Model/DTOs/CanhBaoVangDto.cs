
namespace QlyDaoTao.Model.DTOs
{
    public class CanhBaoVangDto
    {
        public int Id { get; set; }

        public string MaSV { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public decimal TyLeVang { get; set; }

        public decimal NguongCanhBao { get; set; }

        public string NoiDung { get; set; } = string.Empty;

        public bool TrangThai { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
