using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class WindPowerPlatformsController : ControllerBase
    {
        public WindPowerPlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
              Console.WriteLine("--> Inbound POST # Command Service");  

              return Ok("Inbound test of from WindPowerPlatforms Controller");            
        }
    }
}