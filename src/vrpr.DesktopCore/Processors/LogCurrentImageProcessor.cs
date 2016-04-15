using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class LogCurrentImageProcessor : IProcessor<Mat, Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public LogCurrentImageProcessor(IDebugLogger debugLogger)
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
