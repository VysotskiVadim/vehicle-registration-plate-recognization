using System;

namespace vrpr.DesktopCore.DebugLog
{
    public interface IDebugLogger
    {
#pragma warning disable 618
        void Log(Action<IDebugLogBuilder> logAction);
#pragma warning restore 618
    }
}
