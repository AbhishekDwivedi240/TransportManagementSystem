using Microsoft.EntityFrameworkCore;
using TransportManagementSystem.Model;

namespace TransportManagementSystem.Data
{
    public class ApDb:DbContext
    {
        public ApDb(DbContextOptions<ApDb>options):base(options) { }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
