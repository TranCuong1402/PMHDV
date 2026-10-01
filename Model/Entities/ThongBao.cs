namespace QlyDaoTao.Model.Entities
{
    public class ThongBao
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string TieuDe { get; set; } = null!;

        public string NoiDung { get; set; } = null!;

        public string? LoaiThongBao { get; set; }

        public bool DaDoc { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}