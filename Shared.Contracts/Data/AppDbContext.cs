using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Entidades;

namespace Shared.Contracts.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Propuesta> Propuestas { get; set; }
    }
}
