using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public interface IDiemService
    {
        Task NhapDiemAsync(NhapDiemRequest request);

        Task SuaDiemAsync(
            string maSV,
            string maLopHocPhan,
            SuaDiemRequest request);
        Task<DiemTongKetDto> TinhDiemTongKetAsync(
        string maSV,
        string maLopHocPhan);

        Task<List<XemDiemDto>> XemDiemSinhVienAsync(string maSV);
    }
}