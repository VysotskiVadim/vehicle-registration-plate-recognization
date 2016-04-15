namespace vrpr.WebUi.Logger
{
    public class DebugLogModel 
    {
        public LogType Type { get; set; }

        public string Content { get; set; }

        public enum LogType
        {
            String, Base64Image
        }
    }
}
