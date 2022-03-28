using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using WindPowerPlatformService.Data;

namespace WindPowerPlatformService.SyncDataServices.Grpc
{
    public class GrpcWindPowerPlatformService : GrpcWindPowerPlatform.GrpcWindPowerPlatformBase
    {
        private readonly IWindPowerPlatformRepo _repository;
        private readonly IMapper _mapper;

        public GrpcWindPowerPlatformService(IWindPowerPlatformRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<WindPowerPlatformResponse> GetAllWindPowerPlatforms(GetAllRequest request, ServerCallContext context)
        {
            var response = new WindPowerPlatformResponse();
            var platforms = _repository.GetAllPlatforms();

            foreach (var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcWindPowerPlatformModel>(plat));
            }

            return Task.FromResult(response);
        }
    }
}