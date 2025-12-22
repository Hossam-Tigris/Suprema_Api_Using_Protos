using Grpc.Net.Client;
using Gsdk.Connect;

namespace Suprema_Api_Using_Protos.Services
{
    public class ConnectSvc
    {
        private Connect.ConnectClient connectClient;


        public ConnectSvc(GrpcChannel channel)
        {
            connectClient = new Connect.ConnectClient(channel);

        }

        public bool IsDeviceConnected(uint deviceId)
        {
            var list = connectClient.GetDeviceList(new GetDeviceListRequest());

            var device = list.DeviceInfos
                .FirstOrDefault(d => d.DeviceID == deviceId);

            return device != null && device.Status == Status.TcpConnected;
        }
        public GetDeviceListResponse getlist()
        {

            return connectClient.GetDeviceList(new GetDeviceListRequest());

        }

        public uint Connect(string deviceAddr, int devicePort, bool ssl)
        {
            var connectInfo = new ConnectInfo
            {
                IPAddr = deviceAddr,
                Port = devicePort,
                UseSSL = ssl
            };

            var request = new ConnectRequest { ConnectInfo = connectInfo };
            var response = connectClient.Connect(request);

            return response.DeviceID;
        }

        public async Task<Dictionary<uint, string>> ConnectDevicesAsync(List<(string ip, int port, bool useSSL)> devices)
        {
            var request = new AddAsyncConnectionRequest();

            foreach (var device in devices)
            {
                request.ConnectInfos.Add(new AsyncConnectInfo
                {
                    IPAddr = device.ip,
                    Port = device.port,
                    UseSSL = device.useSSL
                });
            }

            await connectClient.AddAsyncConnectionAsync(request);

            var deviceList = await connectClient.GetDeviceListAsync(
                new GetDeviceListRequest());

            var result = new Dictionary<uint, string>();

            foreach (var dev in deviceList.DeviceInfos)
            {
                result[dev.DeviceID] = dev.Status.ToString();
                Console.WriteLine(
                    $"DeviceID={dev.DeviceID} {dev.IPAddr}:{dev.Port} Status={dev.Status}");
            }

            return result;
        }


        public void Disconnect(uint deviceID)
        {
            var request = new DisconnectRequest();
            request.DeviceIDs.Add(deviceID);
            connectClient.Disconnect(request);
        }
    }
}
