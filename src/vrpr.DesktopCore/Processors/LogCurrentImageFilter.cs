using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class LogCurrentImageFilter : IFilter<Mat, Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public LogCurrentImageFilter(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Mat> Process(Mat input)
        {
            _debugLogger.Log(debugBuilder => debugBuilder.AddMessage("Current image is:").AddImage(input));
            return Result<Mat>.Ok(input);
        }
    }
}
