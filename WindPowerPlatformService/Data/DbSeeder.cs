using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WindPowerPlatformService.Models;

namespace  WindPowerPlatformService.Data
{
    public static class DbSeeder
    {
        public static void PreparePopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Attempting to apply migrations...");
                
                try
                {
                   context.Database.Migrate(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if(!context.WindPowerPlatforms.Any())
            {
                Console.WriteLine("--> Seeding data...");

                context.WindPowerPlatforms.AddRange(
                    new WindPowerPlatform(){Name = "Vestas", Manufacturer = "Vestas", Description = "Danish wind power company"},
                    new WindPowerPlatform(){Name = "Siemens Gamesa", Manufacturer = "Siemens_Gamesa", Description = "German manufactural company"},
                    new WindPowerPlatform(){Name = "Nordex", Manufacturer = "Nordex", Description = "German wind power company"}
                );

                context.SaveChanges();
            } 
            else
            {
                Console.WriteLine("--> We already have data");
            }
        } 
    }
}
