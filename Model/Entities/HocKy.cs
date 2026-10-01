namespace QlyDaoTao.Model.Entities
{
    public class HocKy
    {
        public string MaHocKy { get; set; } = null!;

        public string TenHocKy { get; set; } = null!;

        public string MaNienKhoa { get; set; } = null!;

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool TrangThai { get; set; }
    }
}