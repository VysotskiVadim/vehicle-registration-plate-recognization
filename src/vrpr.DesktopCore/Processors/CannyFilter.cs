﻿using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class CannyFilter : IFilter<Mat, Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public CannyFilter(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Mat> Process(Mat input)
        {
            CvInvoke.Canny(input, input, 100, 300, 3);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Canny result").AddImage(input));
            return Result<Mat>.Ok(input);
        }
    }
}
