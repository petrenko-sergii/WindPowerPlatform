using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommandsService.Models;
using CommandsService.Dtos;
using WindPowerPlatformService;

namespace CommandsService.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            // Source -> Target
            CreateMap<WindPowerPlatform, WindPowerPlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<WindPowerPlatformPublishedDto, WindPowerPlatform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<GrpcWindPowerPlatformModel, WindPowerPlatform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
                // .ForMember(dest => dest.Manufacturer, opt => opt.Ignore())
                // .ForMember(dest => dest.Description, opt => opt.Ignore());
        }
    }
}