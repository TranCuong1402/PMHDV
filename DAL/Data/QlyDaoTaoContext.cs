using Microsoft.EntityFrameworkCore;
using QlyDaoTao.Model.Entities;

namespace QlyDaoTao.DAL.Data
{
    public class QlyDaoTaoContext : DbContext
    {
        public QlyDaoTaoContext(
            DbContextOptions<QlyDaoTaoContext> options)
            : base(options)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<CanhBaoVang> CanhBaoVangs { get; set; }
        public DbSet<DangKyHocPhan> DangKyHocPhans { get; set; }
        public DbSet<Diem> Diems { get; set; }
        public DbSet<DiemDanh> DiemDanhs { get; set; }
        public DbSet<GiangVien> GiangViens { get; set; }
        public DbSet<HocKy> HocKys { get; set; }
        public DbSet<LichHoc> LichHocs { get; set; }
        public DbSet<LichThi> LichThis { get; set; }
        public DbSet<Lop> Lops { get; set; }
        public DbSet<LopHocPhan> LopHocPhans { get; set; }
        public DbSet<MonHoc> MonHocs { get; set; }
        public DbSet<NienKhoa> NienKhoas { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<PhucKhao> PhucKhaos { get; set; }
        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<TaiKhoan>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<SinhVien>()
                .HasKey(x => x.MaSV);

            modelBuilder.Entity<GiangVien>()
                .HasKey(x => x.MaGiangVien);

            modelBuilder.Entity<MonHoc>()
                .HasKey(x => x.MaMonHoc);

            modelBuilder.Entity<Lop>()
                .HasKey(x => x.MaLop);

            modelBuilder.Entity<NienKhoa>()
                .HasKey(x => x.MaNienKhoa);

            modelBuilder.Entity<HocKy>()
                .HasKey(x => x.MaHocKy);

            modelBuilder.Entity<LopHocPhan>()
                .HasKey(x => x.MaLopHocPhan);

            modelBuilder.Entity<Phong>()
                .HasKey(x => x.MaPhong);

            modelBuilder.Entity<LichHoc>()
                .HasKey(x => x.MaLichHoc);

            modelBuilder.Entity<LichThi>()
                .HasKey(x => x.MaLichThi);

            modelBuilder.Entity<Diem>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<DiemDanh>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<DangKyHocPhan>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<CanhBaoVang>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<PhucKhao>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ThongBao>()
                .HasKey(x => x.Id);

            // SinhVien -> Lop
            modelBuilder.Entity<SinhVien>()
                .HasOne<Lop>()
                .WithMany()
                .HasForeignKey(x => x.MaLop)
                .OnDelete(DeleteBehavior.Restrict);

            // SinhVien -> TaiKhoan
            modelBuilder.Entity<SinhVien>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.IdTaiKhoan)
                .OnDelete(DeleteBehavior.SetNull);

            // GiangVien -> TaiKhoan
            modelBuilder.Entity<GiangVien>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.IdTaiKhoan)
                .OnDelete(DeleteBehavior.SetNull);

            // Lop -> NienKhoa
            modelBuilder.Entity<Lop>()
                .HasOne<NienKhoa>()
                .WithMany()
                .HasForeignKey(x => x.MaNienKhoa)
                .OnDelete(DeleteBehavior.Restrict);

            // HocKy -> NienKhoa
            modelBuilder.Entity<HocKy>()
                .HasOne<NienKhoa>()
                .WithMany()
                .HasForeignKey(x => x.MaNienKhoa)
                .OnDelete(DeleteBehavior.Restrict);

            // LopHocPhan
            modelBuilder.Entity<LopHocPhan>()
                .HasOne<MonHoc>()
                .WithMany()
                .HasForeignKey(x => x.MaMonHoc)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LopHocPhan>()
                .HasOne<HocKy>()
                .WithMany()
                .HasForeignKey(x => x.MaHocKy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LopHocPhan>()
                .HasOne<GiangVien>()
                .WithMany()
                .HasForeignKey(x => x.MaGiangVien)
                .OnDelete(DeleteBehavior.Restrict);

            // LichHoc
            modelBuilder.Entity<LichHoc>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LichHoc>()
                .HasOne<Phong>()
                .WithMany()
                .HasForeignKey(x => x.MaPhong)
                .OnDelete(DeleteBehavior.SetNull);

            // LichThi
            modelBuilder.Entity<LichThi>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LichThi>()
                .HasOne<Phong>()
                .WithMany()
                .HasForeignKey(x => x.MaPhong)
                .OnDelete(DeleteBehavior.SetNull);

            // Diem
            modelBuilder.Entity<Diem>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Diem>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .OnDelete(DeleteBehavior.Cascade);

            // DiemDanh
            modelBuilder.Entity<DiemDanh>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiemDanh>()
                .HasOne<LichHoc>()
                .WithMany()
                .HasForeignKey(x => x.MaLichHoc)
                .OnDelete(DeleteBehavior.Cascade);

            // DangKyHocPhan
            modelBuilder.Entity<DangKyHocPhan>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DangKyHocPhan>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .OnDelete(DeleteBehavior.Cascade);

            // CanhBaoVang
            modelBuilder.Entity<CanhBaoVang>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CanhBaoVang>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .OnDelete(DeleteBehavior.Cascade);

            // PhucKhao
            modelBuilder.Entity<PhucKhao>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhucKhao>()
                .HasOne<Diem>()
                .WithMany()
                .HasForeignKey(x => x.MaDiem)
                .OnDelete(DeleteBehavior.Restrict);

            // AuditLog -> TaiKhoan
            modelBuilder.Entity<AuditLog>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // ThongBao -> TaiKhoan
            modelBuilder.Entity<ThongBao>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}