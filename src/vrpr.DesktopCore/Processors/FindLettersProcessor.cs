using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class FindLettersProcessor : IProcessor<Mat, IEnumerable<Mat>>
    {
        private readonly IDebugLogger _debugLogger;

        public FindLettersProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<IEnumerable<Mat>> Process(Mat input)
        {
            var binarized = new Mat();
            CvInvoke.AdaptiveThreshold(input, binarized, 255, AdaptiveThresholdType.MeanC, ThresholdType.Binary, 7, 5);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Binarized image").AddImage(binarized));

            var afterCanny = new Mat();
            CvInvoke.Canny(binarized, afterCanny, 1, 3);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Canny result").AddImage(afterCanny));

            return Result.Ok(new [] { input }.AsEnumerable());
        }
    }
}
