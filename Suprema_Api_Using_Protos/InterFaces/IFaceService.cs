using Gsdk.Face;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IFaceService
    {
        Task<FaceData> ScanFaceAsync(uint deviceID, FaceEnrollThreshold faceEnroll);
        Task<FaceConfig> GetConfigAsync(uint deviceID);
        Task<bool> SetConfigAsync(uint deviceID, FaceConfig config);
    }
}
