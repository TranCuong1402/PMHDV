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

        // =========================
        // DbSet
        // =========================

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

            // =====================================================
            // TÊN BẢNG
            // =====================================================

            modelBuilder.Entity<AuditLog>().ToTable("AuditLog");
            modelBuilder.Entity<CanhBaoVang>().ToTable("CanhBaoVang");
            modelBuilder.Entity<DangKyHocPhan>().ToTable("DangKyHocPhan");
            modelBuilder.Entity<Diem>().ToTable("Diem");
            modelBuilder.Entity<DiemDanh>().ToTable("DiemDanh");
            modelBuilder.Entity<GiangVien>().ToTable("GiangVien");
            modelBuilder.Entity<HocKy>().ToTable("HocKy");
            modelBuilder.Entity<LichHoc>().ToTable("LichHoc");
            modelBuilder.Entity<LichThi>().ToTable("LichThi");
            modelBuilder.Entity<Lop>().ToTable("Lop");
            modelBuilder.Entity<LopHocPhan>().ToTable("LopHocPhan");
            modelBuilder.Entity<MonHoc>().ToTable("MonHoc");
            modelBuilder.Entity<NienKhoa>().ToTable("NienKhoa");
            modelBuilder.Entity<Phong>().ToTable("Phong");
            modelBuilder.Entity<PhucKhao>().ToTable("PhucKhao");
            modelBuilder.Entity<SinhVien>().ToTable("SinhVien");
            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<ThongBao>().ToTable("ThongBao");


            // =====================================================
            // PRIMARY KEY
            // =====================================================

            modelBuilder.Entity<AuditLog>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<CanhBaoVang>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<DangKyHocPhan>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Diem>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<DiemDanh>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<GiangVien>()
                .HasKey(x => x.MaGiangVien);

            modelBuilder.Entity<HocKy>()
                .HasKey(x => x.MaHocKy);

            modelBuilder.Entity<LichHoc>()
                .HasKey(x => x.MaLichHoc);

            modelBuilder.Entity<LichThi>()
                .HasKey(x => x.MaLichThi);

            modelBuilder.Entity<Lop>()
                .HasKey(x => x.MaLop);

            modelBuilder.Entity<LopHocPhan>()
                .HasKey(x => x.MaLopHocPhan);

            modelBuilder.Entity<MonHoc>()
                .HasKey(x => x.MaMonHoc);

            modelBuilder.Entity<NienKhoa>()
                .HasKey(x => x.MaNienKhoa);

            modelBuilder.Entity<Phong>()
                .HasKey(x => x.MaPhong);

            modelBuilder.Entity<PhucKhao>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<SinhVien>()
                .HasKey(x => x.MaSV);

            modelBuilder.Entity<TaiKhoan>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<ThongBao>()
                .HasKey(x => x.Id);


            // =====================================================
            // AUDIT LOG
            // FK_AuditLog_TaiKhoan
            // AuditLog.UserId -> TaiKhoan.Id
            // =====================================================

            modelBuilder.Entity<AuditLog>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_AuditLog_TaiKhoan")
                .OnDelete(DeleteBehavior.SetNull);


            // =====================================================
            // CANH BAO VANG
            // =====================================================

            modelBuilder.Entity<CanhBaoVang>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .HasConstraintName("FK_CanhBaoVang_SinhVien")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CanhBaoVang>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .HasConstraintName("FK_CanhBaoVang_LopHocPhan")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // DANG KY HOC PHAN
            // =====================================================

            modelBuilder.Entity<DangKyHocPhan>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .HasConstraintName("FK_DangKyHocPhan_SinhVien")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DangKyHocPhan>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .HasConstraintName("FK_DangKyHocPhan_LopHocPhan")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // DIEM
            // =====================================================

            modelBuilder.Entity<Diem>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .HasConstraintName("FK_Diem_SinhVien")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Diem>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .HasConstraintName("FK_Diem_LopHocPhan")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // DIEM DANH
            // =====================================================

            modelBuilder.Entity<DiemDanh>()
                .HasOne<LichHoc>()
                .WithMany()
                .HasForeignKey(x => x.MaLichHoc)
                .HasConstraintName("FK_DiemDanh_LichHoc")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DiemDanh>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .HasConstraintName("FK_DiemDanh_SinhVien")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // GIANG VIEN
            // =====================================================

            modelBuilder.Entity<GiangVien>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.IdTaiKhoan)
                .HasConstraintName("FK_GiangVien_TaiKhoan")
                .OnDelete(DeleteBehavior.SetNull);


            // =====================================================
            // HOC KY
            // =====================================================

            modelBuilder.Entity<HocKy>()
                .HasOne<NienKhoa>()
                .WithMany()
                .HasForeignKey(x => x.MaNienKhoa)
                .HasConstraintName("FK_HocKy_NienKhoa")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // LICH HOC
            // =====================================================

            modelBuilder.Entity<LichHoc>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .HasConstraintName("FK_LichHoc_LopHocPhan")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichHoc>()
                .HasOne<Phong>()
                .WithMany()
                .HasForeignKey(x => x.MaPhong)
                .HasConstraintName("FK_LichHoc_Phong")
                .OnDelete(DeleteBehavior.SetNull);


            // =====================================================
            // LICH THI
            // =====================================================

         
            modelBuilder.Entity<LichThi>()
                .HasKey(x => x.MaLichThi);

            modelBuilder.Entity<LichThi>()
                .Property(x => x.MaLichThi)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<LichThi>()
                .HasOne<LopHocPhan>()
                .WithMany()
                .HasForeignKey(x => x.MaLopHocPhan)
                .HasConstraintName("FK_LichThi_LopHocPhan")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LichThi>()
                .HasOne<Phong>()
                .WithMany()
                .HasForeignKey(x => x.MaPhong)
                .HasConstraintName("FK_LichThi_Phong")
                .OnDelete(DeleteBehavior.SetNull);


            // =====================================================
            // LOP
            // =====================================================

            modelBuilder.Entity<Lop>()
                .HasOne<NienKhoa>()
                .WithMany()
                .HasForeignKey(x => x.MaNienKhoa)
                .HasConstraintName("FK_Lop_NienKhoa")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // LOP HOC PHAN
            // =====================================================

            modelBuilder.Entity<LopHocPhan>()
                .HasOne<GiangVien>()
                .WithMany()
                .HasForeignKey(x => x.MaGiangVien)
                .HasConstraintName("FK_LopHocPhan_GiangVien")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LopHocPhan>()
                .HasOne<HocKy>()
                .WithMany()
                .HasForeignKey(x => x.MaHocKy)
                .HasConstraintName("FK_LopHocPhan_HocKy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LopHocPhan>()
                .HasOne<MonHoc>()
                .WithMany()
                .HasForeignKey(x => x.MaMonHoc)
                .HasConstraintName("FK_LopHocPhan_MonHoc")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // PHUC KHAO
            // =====================================================

            modelBuilder.Entity<PhucKhao>()
                .HasOne<Diem>()
                .WithMany()
                .HasForeignKey(x => x.MaDiem)
                .HasConstraintName("FK_PhucKhao_Diem")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PhucKhao>()
                .HasOne<SinhVien>()
                .WithMany()
                .HasForeignKey(x => x.MaSV)
                .HasConstraintName("FK_PhucKhao_SinhVien")
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // SINH VIEN
            // =====================================================

            modelBuilder.Entity<SinhVien>()
                .HasOne<Lop>()
                .WithMany()
                .HasForeignKey(x => x.MaLop)
                .HasConstraintName("FK_SinhVien_Lop")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SinhVien>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.IdTaiKhoan)
                .HasConstraintName("FK_SinhVien_TaiKhoan")
                .OnDelete(DeleteBehavior.SetNull);


            // =====================================================
            // THONG BAO
            // =====================================================

            modelBuilder.Entity<ThongBao>()
                .HasOne<TaiKhoan>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .HasConstraintName("FK_ThongBao_TaiKhoan")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}