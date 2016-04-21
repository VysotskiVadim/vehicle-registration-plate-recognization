using System.Drawing;
using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class GaussianBlurProcessor : IProcessor<Mat, Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public GaussianBlurProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Mat> Process(Mat input)
        {
            var level = 5;
            CvInvoke.GaussianBlur(input, input, new Size(level, level), 0);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Gaussian Blur").AddImage(input));
            return Result<Mat>.Ok(input);
        }
    }
}
