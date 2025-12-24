using YourProject.Enums;

public static class EventLogHelper
{
    public static object GetEventInfo(uint eventCode)
    {
        var code = (ushort)eventCode;

        if (!Enum.IsDefined(typeof(Bs2EventCode), code))
        {
            return new
            {
                code = eventCode,
                name = "UnknownEvent",
                category = "Unknown"
            };
        }

        var enumValue = (Bs2EventCode)code;

        return new
        {
            code = eventCode,
            name = enumValue.ToString(),
            category = GetCategory(enumValue)
        };
    }

    private static string GetCategory(Bs2EventCode code)
    {
        var value = (ushort)code;

        if (value >= 0x1000 && value < 0x2000) return "Auth";
        if (value >= 0x2000 && value < 0x3000) return "User";
        if (value >= 0x3000 && value < 0x5000) return "Device";
        if (value >= 0x5000 && value < 0x6000) return "Door";
        if (value >= 0x6000 && value < 0x7000) return "Zone";
        if (value >= 0x7000 && value < 0x8000) return "Lift";

        return "Other";
    }
}