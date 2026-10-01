namespace QlyDaoTao.Model.Entities
{
    public class LichThi
    {
        public int MaLichThi { get; set; }

        public string MaLopHocPhan { get; set; } = null!;

        public DateTime NgayThi { get; set; }

        public TimeSpan GioBatDau { get; set; }

        public TimeSpan GioKetThuc { get; set; }

        public string? MaPhong { get; set; }

        public string? HinhThucThi { get; set; }

        public bool TrangThai { get; set; }
    }
}