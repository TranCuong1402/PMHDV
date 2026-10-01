namespace QlyDaoTao.Model.Entities
{
    public class MonHoc
    {
        public string MaMonHoc { get; set; } = null!;

        public string TenMonHoc { get; set; } = null!;

        public int SoTinChi { get; set; }

        public string? MoTa { get; set; }

        public bool TrangThai { get; set; }
    }
}