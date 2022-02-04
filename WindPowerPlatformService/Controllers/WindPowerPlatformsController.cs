using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WindPowerPlatformService.Dtos;
using WindPowerPlatformService.Models;
using WindPowerPlatformService.Data;
using WindPowerPlatformService.SyncDataServices.Http;

namespace WindPowerPlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindPowerPlatformsController : ControllerBase
    {
        private readonly IWindPowerPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly  ICommandDataClient _commandDataClient;
     
        public WindPowerPlatformsController(
            IWindPowerPlatformRepo repository, 
            IMapper mapper,
            ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WindPowerPlatformReadDto>> GetWindPowerPlatforms()
        {
            Console.WriteLine("--> Gettings WindPowerPlatforms...");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<WindPowerPlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<WindPowerPlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);

            if(platformItem != null)
            {
                return Ok(_mapper.Map<WindPowerPlatformReadDto>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<WindPowerPlatformReadDto>> CreatePlatform(WindPowerPlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<WindPowerPlatform>(platformCreateDto);

            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<WindPowerPlatformReadDto>(platformModel);
            
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id }, platformReadDto);
        }
    }
}