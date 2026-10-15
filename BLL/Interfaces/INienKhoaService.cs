
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface INienKhoaService
    {
        Task<List<NienKhoa>> GetAllAsync();
        Task<NienKhoa?> GetByIdAsync(string maNienKhoa);
        Task<List<NienKhoa>> SearchAsync(string keyword);
        Task<(bool Success, string Message)> AddAsync(NienKhoa nienKhoa);
        Task<(bool Success, string Message)> UpdateAsync(
            string maNienKhoa, NienKhoa nienKhoa);
        Task<(bool Success, string Message)> DeleteAsync(string maNienKhoa);
    }
}
