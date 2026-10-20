
namespace QlyDaoTao.Model.DTOs
{
    public class TyLeVangDto
    {
        public string MaSV { get; set; } = string.Empty;

        public string MaLopHocPhan { get; set; } = string.Empty;

        public int TongSoBuoi { get; set; }

        public int SoBuoiVang { get; set; }

        public int SoBuoiCoPhep { get; set; }

        public int SoBuoiCoMat { get; set; }

        public int SoBuoiMuon { get; set; }

        public decimal TyLeVang { get; set; }
    }
}
