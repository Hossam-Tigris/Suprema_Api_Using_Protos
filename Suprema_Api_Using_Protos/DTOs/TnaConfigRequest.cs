using Gsdk.Tna;
using System.Collections.Generic;

namespace Suprema_Api_Using_Protos.DTOs
{
    public class TnaConfigRequest
    {
        public Mode Mode { get; set; }
        public Key Key { get; set; }
        public bool IsRequired { get; set; }
        public List<uint> Schedules { get; set; } = new();
        public List<string> Labels { get; set; } = new();
    }
}
