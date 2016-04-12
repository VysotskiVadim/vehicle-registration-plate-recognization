using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace vrpr.DesktopCore.DebugLog
{
    public interface IDebugLogBuilder
    {
        IDebugLogBuilder AddString();

        IDebugLogBuilder AddImage(Mat image);
    }
}
