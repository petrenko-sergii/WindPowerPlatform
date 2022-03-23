using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class WindPowerPlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public WindPowerPlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WindPowerPlatformReadDto>> GetPlatforms()
        {
           Console.WriteLine("--> Getting Platforms from CommandService");

           var platformItems = _repository.GetAllPlatforms();

           return Ok(_mapper.Map<IEnumerable<WindPowerPlatformReadDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
              Console.WriteLine("--> Inbound POST # Command Service");  

              return Ok("Inbound test of from WindPowerPlatforms Controller");            
        }
    }
}