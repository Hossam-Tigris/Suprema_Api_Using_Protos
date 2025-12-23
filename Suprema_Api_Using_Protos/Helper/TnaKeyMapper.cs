public static class TnaKeyMapper
{
    public static string GetName(Gsdk.Tna.Key key)
    {
        int value = (int)key;

        return value switch
        {
            0 => "No Reasone",

            1  => $"Check In",
            2  => $"Check In",
            3  => $"Break In",
            4  => $"Break Out",
            5 => $"Overtime In",
            6  => $"Overtime Out",
            7  => $"Mission",
            8  => $"Sick Leave",
            9  => $"Permission",
            10  => $"Work From Home",
 
            _ => "Unknown"
        };
    }

}
