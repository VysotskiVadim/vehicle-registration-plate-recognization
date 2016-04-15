using System.Collections.Generic;

namespace vrpr.WebUi.Logger
{
    public interface IDebugLogProvier
    {
        IList<DebugLogModel> GetLog();
    }
}
