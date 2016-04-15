using System;

#pragma warning disable 618
namespace vrpr.DesktopCore.DebugLog
{
    public class DebugLogger : IDebugLogger
    {
        private readonly IDebugLogBuilder _debugLogBuilder;

        public DebugLogger(IDebugLogBuilder debugLogBuilder)

        {
            _debugLogBuilder = debugLogBuilder;
        }

        public void Log(Action<IDebugLogBuilder> logAction)
        {
            logAction(_debugLogBuilder);
        }
    }
}
#pragma warning restore 618
