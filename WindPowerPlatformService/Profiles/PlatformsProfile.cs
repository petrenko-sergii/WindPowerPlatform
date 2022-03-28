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

            CreateMap<WindPowerPlatform, GrpcWindPowerPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}