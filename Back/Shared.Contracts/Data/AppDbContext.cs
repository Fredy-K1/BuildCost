using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Entidades;

namespace Shared.Contracts.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<DatosM2> DatosM2 { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Propuesta> Propuestas { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder
        )
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DatosM2>()
                .Property(d => d.Value)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Material>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Propuesta>()
                .Property(p => p.Costo)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Material>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.ContratistaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.ContratistaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DatosM2>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(d => d.ContratistaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Material>()
                .HasIndex(m => m.ContratistaId);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ContratistaId);

            modelBuilder.Entity<DatosM2>()
                .HasIndex(d => d.ContratistaId);
        }
    }
}