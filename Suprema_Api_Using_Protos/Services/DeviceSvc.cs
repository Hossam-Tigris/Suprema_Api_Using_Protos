using Grpc.Net.Client;
using Gsdk.Device;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class DeviceSvc : IDeviceService
    {
        private readonly Device.DeviceClient _client;
        private readonly uint _deviceId;


        public DeviceSvc(GrpcChannel channel, uint deviceId)
        {
            _client = new Device.DeviceClient(channel);
            _deviceId = deviceId;
        }

        public async Task RebootDeviceAsync(uint deviceID)
        {
            var request = new RebootRequest { DeviceID = deviceID };
            await _client.RebootAsync(request);
        }

        public async Task FactoryResetAsync(uint deviceID)
        {
            var request = new FactoryResetRequest
            {
                DeviceID = deviceID
            };

            try
            {
                await _client.FactoryResetAsync(request);
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw new Exception($"Cannot perform factory reset: {ex.Status.Detail}");
            }
        }
        public async Task<FactoryInfo> GetDeviceInfoAsync()
        {
            try
            {
                var response = await _client.GetInfoAsync(new GetInfoRequest { DeviceID = _deviceId });
                return response.Info;
            }
            catch (Grpc.Core.RpcException ex)
            {
                throw new Exception($"Cannot get device info: {ex.Status.Detail}");
            }
        }

    }
}