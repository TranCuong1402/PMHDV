using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.BLL.Interfaces
{
    public interface ILopService
    {
        Task<List<Lop>> GetAllAsync();
        Task<Lop?> GetByIdAsync(string maLop);
        Task<List<Lop>> SearchAsync(string keyword);

        Task<(bool Success, string Message)> AddAsync(Lop lop);

        Task<(bool Success, string Message)> UpdateAsync(
            string maLop,
            Lop lop);

        Task<(bool Success, string Message)> DeleteAsync(
            string maLop);
    }
}   