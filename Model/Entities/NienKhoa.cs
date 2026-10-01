namespace QlyDaoTao.Model.Entities
{
    public class NienKhoa
    {
        public string MaNienKhoa { get; set; } = null!;

        public string TenNienKhoa { get; set; } = null!;

        public int NamBatDau { get; set; }

        public int NamKetThuc { get; set; }

        public bool TrangThai { get; set; }
    }
}