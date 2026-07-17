using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Entities;

namespace Shared.Contracts.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
}