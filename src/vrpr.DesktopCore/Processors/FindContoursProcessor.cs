using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class FindContoursProcessor : IProcessor<Mat, Point[][]>
    {
        private readonly IDebugLogger _debugLogger;

        public FindContoursProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Point[][]> Process(Mat input)
        {
            var contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(input, contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
            var contours = contoursDetected.ToArrayOfArray();
            Result<Point[][]> result;
            if (contours.Length > 0)
            {
                result = Result.Ok(contours);
            }
            else
            {
                result = Result.Fail<Point[][]>("Contours hasn't been found");
            }

            _debugLogger.Log(logBuilder =>
            {
                if (result.Success)
                {
                    var imageToDrawContours = new Mat(input.Size, DepthType.Cv8U, 3);
                    imageToDrawContours.SetTo(new MCvScalar(0, 0, 0));
                    var random = new Random();
                    for (int i = 0; i < contours.Length; i++)
                    {
                        CvInvoke.DrawContours(imageToDrawContours, contoursDetected, i, new MCvScalar(random.Next(255), random.Next(255), random.Next(255)));
                    }
                    logBuilder.AddMessage($"Found {contours.Length} contours").AddImage(imageToDrawContours);
                }
                else
                {
                    logBuilder.AddMessage(result.Error);
                }
            });

            return result;
        }
    }
}
