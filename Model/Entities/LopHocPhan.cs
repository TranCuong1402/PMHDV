namespace QlyDaoTao.Model.Entities
{
    public class LopHocPhan
    {
        public string MaLopHocPhan { get; set; } = null!;

        public string MaMonHoc { get; set; } = null!;

        public string MaHocKy { get; set; } = null!;

        public string MaGiangVien { get; set; } = null!;

        public int SiSoToiDa { get; set; }

        public bool TrangThai { get; set; }
    }
}