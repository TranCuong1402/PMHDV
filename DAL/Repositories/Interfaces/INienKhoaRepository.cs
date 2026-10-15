using QlyDaoTao.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface INienKhoaRepository
    {
        Task<List<NienKhoa>> GetAllAsync();
        Task<NienKhoa?> GetByIdAsync(string maNienKhoa);
        Task<List<NienKhoa>> SearchAsync(string keyword);
        Task AddAsync(NienKhoa nienKhoa);
        Task UpdateAsync(NienKhoa nienKhoa);
        Task DeleteAsync(string maNienKhoa);
        Task<bool> ExistsAsync(string maNienKhoa);
        Task SaveChangesAsync();
    }
}
