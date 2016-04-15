namespace vrpr.WebUi.Logger
{
    public class DebugLogModel 
    {
        public LogType Type { get; set; }

        public string Content { get; set; }

        public bool IsImage => Type == LogType.Base64Image;

        public bool IsString => Type == LogType.String;

        public enum LogType
        {
            String, Base64Image
        }
    }
}
