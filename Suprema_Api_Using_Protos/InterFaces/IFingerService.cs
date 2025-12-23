using Gsdk.Finger;

namespace Suprema_Api_Using_Protos.InterFaces
{

    public interface IFingerService
    {
        Task<ScanResponse> ScanAsync(ScanRequest request);
        Task VerifyAsync(uint deviceID, FingerData fingerData);
        Task<GetImageResponse> GetImageAsync(uint deviceID);
        Task<GetConfigResponse> GetConfigAsync(uint deviceID);
        Task SetConfigAsync(uint deviceID, FingerConfig config);
    }
}
