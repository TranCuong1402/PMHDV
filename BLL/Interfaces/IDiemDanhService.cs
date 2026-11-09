
using QlyDaoTao.Model.DTOs;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Services.Interfaces
{
    public interface IDiemDanhService
    {
        Task<string> DiemDanhAsync(
             DiemDanhRequest request,
             int idTaiKhoan,
             string vaiTro);

        Task<string> SuaDiemDanhAsync(
            int maLichHoc,
            string maSV,
            SuaDiemDanhRequest request,
            int idTaiKhoan,
            string vaiTro);
        Task<List<DiemDanh>> GetAllAsync(
         int idTaiKhoan,
         string vaiTro);

        Task<List<DiemDanh>> GetByLichHocAsync(
            int maLichHoc,
            int idTaiKhoan,
            string vaiTro);
        Task<string> XoaDiemDanhAsync(
    int maLichHoc,
    string maSV,
    int idTaiKhoan,
    string vaiTro);
        Task<List<SinhVienDiemDanhDto>>
    LayDanhSachSinhVienTheoLichHocAsync(
        int maLichHoc,
        int idTaiKhoan,
        string vaiTro
    );

    }
}
