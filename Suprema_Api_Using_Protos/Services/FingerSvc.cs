using Grpc.Net.Client;
using Gsdk.Finger;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class FingerSvc : IFingerService
    {
        private readonly Finger.FingerClient client;
        private readonly uint _deviceId;


        public FingerSvc(GrpcChannel channel, uint deviceId)
        {
            client = new Finger.FingerClient(channel);
            _deviceId = deviceId;
        }

        public async Task<ScanResponse> ScanAsync(ScanRequest request)
        {


            return await client.ScanAsync(request);
        }
        public async Task<GetImageResponse> GetImageAsync(uint deviceID)
        {
            var request = new GetImageRequest { DeviceID = deviceID };
            return await client.GetImageAsync(request);
        }

        public async Task VerifyAsync(uint deviceID, FingerData fingerData)
        {
            var request = new VerifyRequest { DeviceID = deviceID, FingerData = fingerData };
            await client.VerifyAsync(request);
        }

        //public async Task<FingerData> ScanAndVerifyAsync(uint deviceID)
        //{
        //    var scan1 = await ScanAsync(deviceID);
        //    //var scan2 = await ScanAsync(deviceID);

        //    var fingerData = new FingerData
        //    {
        //        Index = 0,
        //        Flag = 0,
        //        Templates =
        //    {
        //        scan1.TemplateData,
        //        //scan2.TemplateData
        //    }
        //    };

        //    //await VerifyAsync(deviceID, fingerData);

        //    return fingerData;
        //}

        public async Task<GetConfigResponse> GetConfigAsync(uint deviceID)
        {
            var request = new GetConfigRequest { DeviceID = deviceID };
            return await client.GetConfigAsync(request);
        }

        public async Task SetConfigAsync(uint deviceID, FingerConfig config)
        {
            var request = new SetConfigRequest { DeviceID = deviceID, Config = config };
            await client.SetConfigAsync(request);
        }


    }
}
