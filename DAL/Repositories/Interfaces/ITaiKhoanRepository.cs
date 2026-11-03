using QlyDaoTao.Model.Entities;

public interface ITaiKhoanRepository
{
    Task<TaiKhoan?> GetByTenDangNhapAsync(string tenDangNhap);
    Task<bool> ExistsByTenDangNhapAsync(string tenDangNhap);
    Task AddAsync(TaiKhoan taiKhoan);
    Task UpdateAsync(TaiKhoan taiKhoan);
    Task<TaiKhoan?> GetByRefreshTokenAsync(string refreshToken);
    Task<TaiKhoan?> GetByEmailAsync(string email);
}