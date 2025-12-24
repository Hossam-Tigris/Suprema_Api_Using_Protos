namespace Suprema_Api_Using_Protos.DTOs
{
    public class EventLogDto
    {
        public uint Id { get; set; }
        public DateTime Time { get; set; }
        public uint DeviceId { get; set; }
        public string UserId { get; set; }

        public uint EventCode { get; set; }
        public string EventCodeName { get; set; }

        public int TnaKey { get; set; }
        public string TnaKeyName { get; set; }

        public bool HasImage { get; set; }
    }

}
