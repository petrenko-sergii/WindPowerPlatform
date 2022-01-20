using System.Collections.Generic;
using WindPowerPlatformService.Models;

namespace  WindPowerPlatformService.Data 
{
    public interface IWindPowerPlatformRepo
    {
        bool SaveChanges();
        
        IEnumerable<WindPowerPlatform> GetAllPlatforms();

        WindPowerPlatform GetPlatformById(int id);

        void CreatePlatform(WindPowerPlatform platform);
    }
}