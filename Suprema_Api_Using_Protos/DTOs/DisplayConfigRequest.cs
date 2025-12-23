using Gsdk.Display;

namespace Suprema_Api_Using_Protos.DTOs
{
    public class DisplayConfigRequest
    {
        public LanguageType? Language { get; set; }
        public BackgroundType? Background { get; set; }
        public BackgroundTheme? Theme { get; set; }

        public uint? Volume { get; set; }
        public bool? UseVoice { get; set; }

        public DateFormat? DateFormat { get; set; }
        public TimeFormat? TimeFormat { get; set; }
        public bool? ShowDateTime { get; set; }

        public uint? MenuTimeout { get; set; }
        public uint? MsgTimeout { get; set; }
        public uint? BacklightTimeout { get; set; }

        public bool? UseUserPhrase { get; set; }
    }
}
