using HealthyMomAndBaby.Entity;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.DataContext
{
    public class HealthyMomAndBabyContext : DbContext
    {
        public HealthyMomAndBabyContext(DbContextOptions<HealthyMomAndBabyContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
