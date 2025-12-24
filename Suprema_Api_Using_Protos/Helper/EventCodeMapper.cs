public static class EventCodeMapper
{
    private static readonly Dictionary<uint, string> _events = new()
    {
        { 0x1100, "Access Success" },
        { 0x1101, "Access Fail" },

        { 0x1200, "Authentication Success" },
        { 0x1201, "Authentication Fail" },

        { 0x2100, "Door Opened" },
        { 0x2101, "Door Closed" },

        { 0x3100, "Alarm Triggered" },

        { 0x4100, "System Restart" }
    };

    public static string GetName(uint eventCode)
    {
        uint mainCode = eventCode & 0xFF00;

        return _events.TryGetValue(mainCode, out var name)
            ? name
            : $"Unknown Event (0x{eventCode:X})";
    }
}
