using Grpc.Net.Client;
using Gsdk.Device;
using Suprema_Api_Using_Protos.Services;

namespace Suprema_Api_Using_Protos.Services
{
    public class DeviceSession
    {
        public uint DeviceID { get; set; }
        public string IP { get; }
        public ServiceFactory Services { get; }

        public DeviceSession(uint deviceID, string ip, GrpcChannel channel)
        {
            DeviceID = deviceID;
            IP = ip;
            Services = new ServiceFactory( channel, DeviceID);
       }



    }
}

