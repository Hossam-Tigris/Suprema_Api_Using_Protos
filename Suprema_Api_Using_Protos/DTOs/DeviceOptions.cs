namespace Suprema_Api_Using_Protos.DTOs
{
    public class DeviceOptions
    {
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
