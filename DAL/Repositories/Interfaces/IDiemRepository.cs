namespace QlyDaoTao.DAL.Repositories
{
    public interface IDiemRepository
    {
        Task<bool> SinhVienExistsAsync(string maSV);

        Task<bool> LopHocPhanExistsAsync(string maLopHocPhan);

        Task<bool> SinhVienDaDangKyAsync(
            string maSV, string maLopHocPhan);

        Task<bool> DiemExistsAsync(
            string maSV, string maLopHocPhan);

        Task AddAsync(QlyDaoTao.Model.Entities.Diem diem);

        Task SaveChangesAsync();
    }
}