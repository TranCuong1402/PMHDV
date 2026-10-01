namespace QlyDaoTao.Model.Entities
{
    public class CanhBaoVang
    {
        public int Id { get; set; }

        public string MaSV { get; set; } = null!;

        public string MaLopHocPhan { get; set; } = null!;

        public decimal TyLeVang { get; set; }

        public decimal NguongCanhBao { get; set; }

        public string NoiDung { get; set; } = null!;

        public bool TrangThai { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}