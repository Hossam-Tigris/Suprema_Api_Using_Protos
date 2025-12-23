public class WeeklyScheduleRequest
{
    public uint DeviceId { get; set; }
    public uint ScheduleId { get; set; }
    public string Name { get; set; } = string.Empty;

    // day -> list of periods
    public Dictionary<int, List<TimePeriodDto>> Days { get; set; } = new();
}

public class TimePeriodDto
{
    public int StartHour { get; set; }
    public int StartMinute { get; set; }
    public int EndHour { get; set; }
    public int EndMinute { get; set; }
}
