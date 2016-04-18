using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class CannyProcessor : IProcessor<Mat, Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public CannyProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Mat> Process(Mat input)
        {
            var afterCanny = new Mat();
            CvInvoke.Canny(input, afterCanny, 1, 3);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Canny result").AddImage(afterCanny));
            return Result<Mat>.Ok(afterCanny);
        }
    }
}
