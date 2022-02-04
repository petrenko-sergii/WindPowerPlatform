using System.Threading.Tasks;
using WindPowerPlatformService.Dtos;

namespace WindPowerPlatformService.SyncDataServices.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(WindPowerPlatformReadDto platform);
    }
}