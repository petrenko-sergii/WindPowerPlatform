using System;
using System.Collections.Generic;
using WindPowerPlatformService.Models;
using System.Linq;

namespace  WindPowerPlatformService.Data 
{
    public class WindPowerPlatformRepo : IWindPowerPlatformRepo
    {
        private readonly AppDbContext _context; 

        public WindPowerPlatformRepo(AppDbContext context)
        {
            _context = context;            
        }
        
        public IEnumerable<WindPowerPlatform> GetAllPlatforms()
        {
            return _context.WindPowerPlatforms.ToList();
        }

        public WindPowerPlatform GetPlatformById(int id)
        {
            //small reminder: use FirstOrDefault() to skip 500-error
            return _context.WindPowerPlatforms.FirstOrDefault(p => p.Id == id);
        }

        public void CreatePlatform(WindPowerPlatform platform)
        {
            if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.WindPowerPlatforms.Add(platform);
        }

        public bool SaveChanges() 
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}