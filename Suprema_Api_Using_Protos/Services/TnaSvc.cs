using Gsdk.Tna;
using Grpc.Net.Client;
using System.Threading.Tasks;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class TnaSvc:ITnaService
    {
        private readonly TNA.TNAClient _client;

        public TnaSvc(GrpcChannel channel)
        {
            _client = new TNA.TNAClient(channel);
        }

        public async Task<TNAConfig> GetConfigAsync(uint deviceId)
        {
            var res = await _client.GetConfigAsync(
                new GetConfigRequest { DeviceID = deviceId });
                
            return res.Config;
        }

        public async Task SetConfigAsync(uint deviceId, TNAConfig config)
        {
            await _client.SetConfigAsync(new SetConfigRequest
            {
                DeviceID = deviceId,
                Config = config
            });
        }

        public async Task SetConfigMultiAsync(IEnumerable<uint> deviceIds, TNAConfig config)
        {
            await _client.SetConfigMultiAsync(new SetConfigMultiRequest
            {
                DeviceIDs = { deviceIds },
                Config = config
            });
        }

        public async Task<GetTNALogResponse> GetLogsAsync( uint deviceId,uint startEventId,uint max)
        {
            return await _client.GetTNALogAsync(new GetTNALogRequest
            {
                DeviceID = deviceId,
                StartEventID = startEventId,
                MaxNumOfLog = max
            });
        }
    }
}
