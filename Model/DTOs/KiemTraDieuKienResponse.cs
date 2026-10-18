
namespace QlyDaoTao.Model.DTOs
{
    public class KiemTraDieuKienResponse
    {
        public bool DuDieuKien { get; set; }

        public string ThongBao { get; set; } = string.Empty;

        public List<string> LyDo { get; set; } = new();
    }
}
