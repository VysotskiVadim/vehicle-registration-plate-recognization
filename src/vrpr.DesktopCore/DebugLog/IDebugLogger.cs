using System;

namespace vrpr.DesktopCore.DebugLog
{
    public interface IDebugLogger
    {
        void Log(Action<IDebugLogBuilder> logAction);
    }
}
