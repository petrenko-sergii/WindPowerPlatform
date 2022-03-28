using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace CommandsService.Data
{
    public static class DbSeeder
    {
        public static void PreparePopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IWindPowerPlatformDataClient>();

                var platforms = grpcClient.ReturnAllWindPowerPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        
        private static void SeedData(ICommandRepo repo, IEnumerable<WindPowerPlatform> platforms)
        {
            Console.WriteLine("--> Seeding new WindPowerPlatforms...");

            foreach (var plat in platforms)
            {
                if(!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }

                repo.SaveChanges();
            }
        }
    }
}