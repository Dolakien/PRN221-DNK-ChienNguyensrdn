using HealthyMomAndBaby.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyMomAndBaby.DataContext
{
    public class HealthyMomAndBabyContext : DbContext
    {
        public HealthyMomAndBabyContext(DbContextOptions<HealthyMomAndBabyContext> options) : base(options)
        {
        }

    }
}
