using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Entidades;

namespace Shared.Contracts.DbContext
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<DatosM2> DatosM2 { get; set; }
    }
}
