using Gsdk.Auth;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IAuthService
    {
        Task SetAuthAsync(uint deviceId, AuthMode mode, uint scheduleId);
        Task<AuthConfig> GetConfigAsync(uint deviceId);
    }
}
