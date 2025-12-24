public static class TnaHelper
{
    public static string GetLabel(
        Gsdk.Tna.Key key,
        IList<string> labels)
    {
        int index = (int)key - 1;

        if (index < 0 || index >= labels.Count)
            return "Unknown";

        return labels[index];
    }
}
