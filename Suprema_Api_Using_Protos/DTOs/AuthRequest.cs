using Gsdk.Auth;

namespace Suprema_Api_Using_Protos.DTOs
{
    public class AuthRequest
    {
        public AuthMode Mode { get; set; }
        public uint ScheduleID { get; set; }
    }
}
