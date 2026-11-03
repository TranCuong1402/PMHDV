using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.Model.Entities;

public class TaiKhoanRepository : ITaiKhoanRepository
{
    private readonly QlyDaoTaoContext _context;

    public TaiKhoanRepository(QlyDaoTaoContext context)
    {
        _context = context;
    }

    public async Task<TaiKhoan?> GetByTenDangNhapAsync(string tenDangNhap)
    {
        return await _context.TaiKhoans
            .FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap);
    }
    public async Task<bool> ExistsByTenDangNhapAsync(string tenDangNhap)
    {
        return await _context.TaiKhoans
            .AnyAsync(x => x.TenDangNhap == tenDangNhap);
    }

    public async Task AddAsync(TaiKhoan taiKhoan)
    {
        await _context.TaiKhoans.AddAsync(taiKhoan);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateAsync(TaiKhoan taiKhoan)
    {
        _context.TaiKhoans.Update(taiKhoan);
        await _context.SaveChangesAsync();
    }

    public async Task<TaiKhoan?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.TaiKhoans
            .FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
    }
    public async Task<TaiKhoan?> GetByEmailAsync(string email)
    {
        return await _context.TaiKhoans
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}