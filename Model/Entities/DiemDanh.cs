namespace QlyDaoTao.Model.Entities
{
    public class DiemDanh
    {
        public int Id { get; set; }

        public int MaLichHoc { get; set; }

        public string MaSV { get; set; } = null!;

        public string TrangThai { get; set; } = null!;

        public DateTime? ThoiGianDiemDanh { get; set; }

        public string? LyDo { get; set; }

        public string? NguoiDiemDanh { get; set; }
    }
}