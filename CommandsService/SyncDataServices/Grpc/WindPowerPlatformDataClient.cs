using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using CommandsService.Models;
using WindPowerPlatformService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class WindPowerPlatformDataClient : IWindPowerPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public WindPowerPlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<WindPowerPlatform> ReturnAllWindPowerPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcWindPowerPlatform"]}");

            var channel = GrpcChannel.ForAddress(_configuration["GrpcWindPowerPlatform"]);
            var client = new GrpcWindPowerPlatform.GrpcWindPowerPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllWindPowerPlatforms(request);

                return _mapper.Map<IEnumerable<WindPowerPlatform>>(reply.Platform);
            }
            catch (Exception ex)
            {
               Console.WriteLine($"--> Could not call GPRC Server {ex.Message}");

               return null;
            }       
        }
    }
}
