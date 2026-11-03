using Microsoft.EntityFrameworkCore;
using QlyDaoTao.DAL.Data;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Repositories
{
    public class PhucKhaoRepository : IPhucKhaoRepository
    {
        private readonly QlyDaoTaoContext _context;

        public PhucKhaoRepository(QlyDaoTaoContext context)
        {
            _context = context;
        }

        public async Task<bool> SinhVienExistsAsync(string maSV)
        {
            return await _context.SinhViens
                .AnyAsync(x => x.MaSV == maSV);
        }

        public async Task<Diem?> GetDiemAsync(int maDiem)
        {
            return await _context.Diems
                .FirstOrDefaultAsync(x => x.Id == maDiem);
        }

        public async Task<bool> DaGuiYeuCauAsync(
            string maSV, int maDiem)
        {
            return await _context.PhucKhaos
                .AnyAsync(x =>
                    x.MaSV == maSV &&
                    x.MaDiem == maDiem &&
                    x.TrangThai == "CHO_DUYET");

        }
        public async Task<List<PhucKhao>> GetAllAsync()
        {
            return await _context.PhucKhaos
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<PhucKhao?> GetByIdAsync(int id)
        {
            return await _context.PhucKhaos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task UpdateAsync(PhucKhao phucKhao)
        {
            _context.PhucKhaos.Update(phucKhao);
            return Task.CompletedTask;
        }
        public async Task<bool> NguoiXuLyExistsAsync(string nguoiXuLy)
        {
            return await _context.GiangViens
                .AnyAsync(x => x.MaGiangVien == nguoiXuLy);
        }
        public Task UpdateDiemAsync(Diem diem)
        {
            _context.Diems.Update(diem);
            return Task.CompletedTask;
        }
        public async Task AddAsync(PhucKhao phucKhao)
        {
            await _context.PhucKhaos.AddAsync(phucKhao);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}