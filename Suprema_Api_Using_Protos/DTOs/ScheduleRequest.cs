namespace Suprema_Api_Using_Protos.DTOs
{
    public class ScheduleRequest
    {
        public uint ScheduleID { get; set; }
        public string Name { get; set; }

        public Dictionary<int, List<(int Start, int End)>> DayTimes { get; set; } = new();
    }
}
