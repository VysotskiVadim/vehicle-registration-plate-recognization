using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class BinarizationProcessor : IProcessor<Mat, Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public BinarizationProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Mat> Process(Mat input)
        {
            CvInvoke.Threshold(input, input, 0, 255, ThresholdType.Otsu);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Binarized image").AddImage(input));
            return Result<Mat>.Ok(input);
        }
    }
}
