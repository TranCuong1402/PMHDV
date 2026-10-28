
namespace QlyDaoTao.Model.DTOs
{
    public class KiemTraTrungLichResponse
    {
        public bool HopLe { get; set; }

        public List<string> DanhSachLoi { get; set; } = new();

        public string ThongBao { get; set; } = string.Empty;
    }
}
