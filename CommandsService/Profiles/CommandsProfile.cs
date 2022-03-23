using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommandsService.Models;
using CommandsService.Dtos;

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
        }
    }
}