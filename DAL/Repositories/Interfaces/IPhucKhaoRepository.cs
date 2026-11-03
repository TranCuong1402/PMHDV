using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public interface IPhucKhaoRepository
    {
        Task<bool> SinhVienExistsAsync(string maSV);

        Task<Diem?> GetDiemAsync(int maDiem);

        Task<bool> DaGuiYeuCauAsync(string maSV, int maDiem);

        Task AddAsync(PhucKhao phucKhao);
        Task UpdateAsync(PhucKhao phucKhao);

        Task<bool> NguoiXuLyExistsAsync(string nguoiXuLy);
        Task UpdateDiemAsync(Diem diem);

        Task SaveChangesAsync();
        Task<List<PhucKhao>> GetAllAsync();

        Task<PhucKhao?> GetByIdAsync(int id);
    }
}