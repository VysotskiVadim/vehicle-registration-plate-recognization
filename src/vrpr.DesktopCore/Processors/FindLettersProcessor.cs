using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
            CvInvoke.Threshold(input, binarized, 0, 255, ThresholdType.Otsu);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Binarized image").AddImage(binarized));

            var afterCanny = new Mat();
            CvInvoke.Canny(binarized, afterCanny, 1, 3);
            _debugLogger.Log(logBuilder => logBuilder.AddMessage("Canny result").AddImage(afterCanny));

            var contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(afterCanny, contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);

            var countours = contoursDetected.ToArrayOfArray();

            var minLetterHeight = (int)(input.Height * 0.3);
            var maxLetterHeight = (int)(input.Height * 0.8);
            var minLetterWidth = (int)(input.Width*0.05);

            var letterCounors = countours.Where(countor =>
            {
                var countorHeight = countor.Max(point => point.Y) - countor.Min(point => point.Y);
                var countorWidth = countor.Max(point => point.X) - countor.Min(point => point.X);
                return countorHeight > minLetterHeight && countorHeight < maxLetterHeight && countorWidth > minLetterWidth;
            }).AsParallel().WithDegreeOfParallelism(2).ToArray();

            _debugLogger.Log(debugLogBuilder =>
            {
                debugLogBuilder.AddMessage($"found letters {letterCounors.Length}");
                var inputWithSelectedLetters = input.Clone();
                for (int i = 0; i < letterCounors.Length; i++)
                {
                    CvInvoke.DrawContours(inputWithSelectedLetters, new VectorOfVectorOfPoint(letterCounors), i, new MCvScalar(255, 255, 255));
                }
                debugLogBuilder.AddImage(inputWithSelectedLetters);
            });

            return Result.Ok(new [] { input }.AsEnumerable());
        }
    }
}
