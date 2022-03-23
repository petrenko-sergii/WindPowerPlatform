using AutoMapper;
using WindPowerPlatformService.Dtos;
using WindPowerPlatformService.Models;

namespace WindPowerPlatformService.Profiles
{
    public class PlatformsProfile : Profile 
    {
        public PlatformsProfile()
        {
            // Source -> to Target
            CreateMap<WindPowerPlatform, WindPowerPlatformReadDto>();
            CreateMap<WindPowerPlatformCreateDto, WindPowerPlatform>();
            CreateMap<WindPowerPlatformReadDto, WindPowerPlatformPublishedDto>();
        }
    }
}