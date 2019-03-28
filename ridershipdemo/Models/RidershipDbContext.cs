using Microsoft.EntityFrameworkCore;

namespace ridershipdemo.Models
{
    public class RidershipDbContext : DbContext
    {
        public RidershipDbContext (DbContextOptions<RidershipDbContext> options)
            :base(options)
            {
                
            }

        public DbSet<ridershipdemo.Models.TripData> Trips {get; set;}
    }
}