namespace QlyDaoTao.Model.Entities
{
    public class LichHoc
    {
        public int MaLichHoc { get; set; }

        public string MaLopHocPhan { get; set; } = null!;

        public DateTime NgayHoc { get; set; }

        public TimeSpan GioBatDau { get; set; }

        public TimeSpan GioKetThuc { get; set; }

        public string? MaPhong { get; set; }

        public string TrangThai { get; set; } = null!;
    }
}