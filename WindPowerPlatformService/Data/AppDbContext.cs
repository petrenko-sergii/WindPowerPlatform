using Microsoft.EntityFrameworkCore;
using WindPowerPlatformService.Models;

namespace  WindPowerPlatformService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }

        public DbSet<WindPowerPlatform> WindPowerPlatforms {get; set;}
    }

}