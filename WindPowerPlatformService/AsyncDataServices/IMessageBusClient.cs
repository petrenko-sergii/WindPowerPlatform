using System;
using System.Collections.Generic;
using System.Linq;
using WindPowerPlatformService.Dtos;

namespace WindPowerPlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewWindPowerPlatform(WindPowerPlatformPublishedDto platformPublishedDto);
    }
}