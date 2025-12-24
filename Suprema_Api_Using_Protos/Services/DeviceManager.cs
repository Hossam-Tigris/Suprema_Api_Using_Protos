namespace Suprema_Api_Using_Protos.Services
{
    public class DeviceManager
    {
        private readonly GatewayClient _gateway;
        private readonly ConnectSvc _connectSvc;
        private readonly object _lock = new();
        public bool GatewayConnected { get; private set; }

        public List<DeviceSession> Devices { get; } = new();

        public DeviceManager(string gatewayCa, string gatewayAddr, int gatewayPort)
        {
            _gateway = new GatewayClient();
            _gateway.Connect(gatewayCa, gatewayAddr, gatewayPort);
            GatewayConnected = true;

            _connectSvc = new ConnectSvc(_gateway.Channel);
        }


        public async Task ConnectDevicesAsync(
            List<(string ip, int port, bool useSSL)> devices)
        {
            foreach (var d in devices)
            {
                await ConnectSingleDeviceAsync(d.ip, d.port, d.useSSL);
            }
        }


        public Task<uint> ConnectSingleDeviceAsync(
            string ip, int port, bool useSSL)
        {
            try
            {
                uint deviceId = _connectSvc.Connect(ip, port, useSSL);

                lock (_lock)
                {
                    Devices.Add(new DeviceSession(
                        deviceId,
                        ip,
                        _gateway.Channel
                        
                    ));
                }

                return Task.FromResult(deviceId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect {ip}: {ex.Message}");
                throw;
            }
        }


        public void DisconnectDevice(uint deviceId)
        {
            var session = Devices.FirstOrDefault(d => d.DeviceID == deviceId);
            if (session == null)
                throw new InvalidOperationException("Device not found");

            _connectSvc.Disconnect(deviceId);

            lock (_lock)
            {
                Devices.Remove(session);
            }
        }


        public async Task<uint> ReconnectDeviceAsync(uint deviceId)
        {
            var session = Devices.FirstOrDefault(d => d.DeviceID == deviceId);
            if (session == null)
                throw new InvalidOperationException("Device not found");

            DisconnectDevice(deviceId);

            return await ConnectSingleDeviceAsync(
                session.IP,
                51211,
                false
            );
        }

        public DeviceSession? GetDevice(uint deviceId)
        {
            return Devices.FirstOrDefault(d => d.DeviceID == deviceId);
        }
    }
}
