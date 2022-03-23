using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WindPowerPlatformService.Dtos;
using WindPowerPlatformService.Models;
using WindPowerPlatformService.Data;
using WindPowerPlatformService.AsyncDataServices;
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
        private readonly  IMessageBusClient _messageBusClient;
     
        public WindPowerPlatformsController(
            IWindPowerPlatformRepo repository, 
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
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
            
            // Send Sync Message
            try
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            // Send Async Message
             try
            {
                var platformPublishedDto = _mapper.Map<WindPowerPlatformPublishedDto>(platformReadDto);
                platformPublishedDto.Event = "WindPowerPlatform_Published";
                _messageBusClient.PublishNewWindPowerPlatform(platformPublishedDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            } 


            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDto.Id }, platformReadDto);
        }
    }
}