namespace Suprema_Api_Using_Protos.DTOs
{
    public class ConnectDeviceRequest
    {
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
