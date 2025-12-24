using Grpc.Net.Client;
using Gsdk.Auth;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class AuthSvc:IAuthService
    {
        private readonly Auth.AuthClient _client;

        public AuthSvc(GrpcChannel channel)
        {
            _client = new Auth.AuthClient(channel);
        }

        public async Task SetAuthAsync(uint deviceId, AuthMode mode, uint scheduleId)
        {
            var config = new AuthConfig
            {
                UseFullAccess = false,
                MatchTimeout = 5,
                AuthTimeout = 5
            };

            config.AuthSchedules.Add(new AuthSchedule
            {
                Mode = mode,
                ScheduleID = scheduleId
            });

            await _client.SetConfigAsync(new SetConfigRequest
            {
                DeviceID = deviceId,
                Config = config
            });
        }

        public async Task<AuthConfig> GetConfigAsync(uint deviceId)
        {
            var res = await _client.GetConfigAsync(new GetConfigRequest
            {
                DeviceID = deviceId
            });

            return res.Config;
        }
    }
}
