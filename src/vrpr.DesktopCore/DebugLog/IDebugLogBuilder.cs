using System;
using Emgu.CV;

namespace vrpr.DesktopCore.DebugLog
{
    [Obsolete("Do not use it directly, use IDebugLogger to access this functionality")]
    public interface IDebugLogBuilder
    {
        IDebugLogBuilder AddMessage(string message);

        IDebugLogBuilder AddImage(Mat image);
    }
}
