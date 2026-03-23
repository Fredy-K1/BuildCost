using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Models; 

namespace CustomerService.Data
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
