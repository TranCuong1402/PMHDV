using QlyDaoTao.Model.DTOs;

namespace QlyDaoTao.BLL.Services
{
    public interface IPhucKhaoService
    {
        // UC35
        Task GuiYeuCauAsync(GuiPhucKhaoRequest request);

        // UC36
        Task<List<PhucKhaoResponse>> GetAllAsync();
        Task<PhucKhaoResponse> GetByIdAsync(int id);

        // UC37
        Task DuyetYeuCauAsync(
            int id, DuyetPhucKhaoRequest request);
        Task TuChoiYeuCauAsync(int id, TuChoiPhucKhaoRequest request);
        Task CapNhatDiemAsync(
    int id, CapNhatDiemPhucKhaoRequest request);
    }
}