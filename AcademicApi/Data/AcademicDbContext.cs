using AcademicApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicApi.Data;

public class AcademicDbContext : DbContext
{
    public AcademicDbContext(DbContextOptions<AcademicDbContext> options) : base(options)
    {
    }

    public DbSet<Dosen> Dosen => Set<Dosen>();

    public DbSet<Mahasiswa> Mahasiswa => Set<Mahasiswa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Dosen>(entity =>
        {
            entity.ToTable("Dosen");
            entity.HasKey(d => d.DosenId);
            entity.Property(d => d.DosenId).ValueGeneratedOnAdd();
            entity.Property(d => d.Nidn).IsRequired().HasMaxLength(20);
            entity.HasIndex(d => d.Nidn).IsUnique();
            entity.Property(d => d.NamaDosen).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Email).IsRequired().HasMaxLength(100);
            entity.Property(d => d.Prodi).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Mahasiswa>(entity =>
        {
            entity.ToTable("Mahasiswa", table =>
                table.HasCheckConstraint("CK_Mahasiswa_Angkatan", "Angkatan BETWEEN 2020 AND 2026"));
            entity.HasKey(m => m.MahasiswaId);
            entity.Property(m => m.MahasiswaId).ValueGeneratedOnAdd();
            entity.Property(m => m.Nim).IsRequired().HasMaxLength(20);
            entity.HasIndex(m => m.Nim).IsUnique();
            entity.Property(m => m.NamaMahasiswa).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Email).IsRequired().HasMaxLength(100);
            entity.Property(m => m.Angkatan).IsRequired();
            entity.Property(m => m.DosenId).IsRequired();
            entity.HasOne(m => m.Dosen)
                .WithMany(d => d.DaftarMahasiswa)
                .HasForeignKey(m => m.DosenId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        });
    }
}
