using Dashboard.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Models
{
    public class DashboardDbContext : DbContext
    {
        public DashboardDbContext(DbContextOptions<DashboardDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}