namespace QlyDaoTao.Model.DTOs
{
    public class ThongBaoThayDoiLichRequest
    {
        public int MaLichHoc { get; set; }

        public string TieuDe { get; set; } = "Thông báo thay đổi lịch học";

        public string NoiDung { get; set; } = string.Empty;
    }
}