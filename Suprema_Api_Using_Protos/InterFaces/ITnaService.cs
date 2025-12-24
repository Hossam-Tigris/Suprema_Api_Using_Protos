using Gsdk.Tna;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface ITnaService
    {
        Task<TNAConfig> GetConfigAsync(uint deviceId);
        Task SetConfigAsync(uint deviceId, TNAConfig config);
        Task SetConfigMultiAsync(IEnumerable<uint> deviceIds, TNAConfig config);
        Task<GetTNALogResponse> GetLogsAsync(uint deviceId, uint startEventId, uint max);

    }
}
