using Emgu.CV;

namespace vrpr.DesktopCore.DebugLog
{
    public interface IDebugLogBuilder
    {
        IDebugLogBuilder AddMessage(string message);

        IDebugLogBuilder AddImage(Mat image);
    }
}
