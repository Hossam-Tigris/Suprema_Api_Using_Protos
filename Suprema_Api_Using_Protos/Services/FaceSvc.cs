using Grpc.Net.Client;
using Gsdk.Face;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class FaceSvc : IFaceService
    {
        private readonly Face.FaceClient FaceClient;
        private readonly uint _deviceId;

        public FaceSvc(GrpcChannel channel, uint deviceId)
        {
            FaceClient = new Face.FaceClient(channel);
            _deviceId = deviceId;
        }


        public async Task<FaceData> ScanFaceAsync(uint deviceID, FaceEnrollThreshold threshold = FaceEnrollThreshold.Bs2FaceEnrollThresholdDefault)
        {
            var request = new ScanRequest
            {
                DeviceID = deviceID,
                EnrollThreshold = threshold
            };

            try
            {
                var response = await FaceClient.ScanAsync(request);
                if (response.FaceData != null)
                {
                    Console.WriteLine($"Face scanned successfully! Index: {response.FaceData.Index}, Templates: {response.FaceData.Templates.Count}");
                }
                else
                {
                    Console.WriteLine("Scan completed but no face data received.");
                }
                return response.FaceData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Face scan failed: {ex.Message}");
                return null;
            }
        }

        public async Task<FaceConfig> GetConfigAsync(uint deviceID)
        {
            var request = new GetConfigRequest { DeviceID = deviceID };
            var response = await FaceClient.GetConfigAsync(request);
            return response.Config;
        }

        public async Task<bool> SetConfigAsync(uint deviceID, FaceConfig config)
        {
            var request = new SetConfigRequest
            {
                DeviceID = deviceID,
                Config = config
            };

            try
            {
                await FaceClient.SetConfigAsync(request);
                Console.WriteLine("Face configuration updated successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to set face config: {ex.Message}");
                return false;
            }
        }
    }
}
