using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class FindContoursProcessor : IProcessor<Mat, Point[][]>
    {
        public Result<Point[][]> Process(Mat input)
        {
            var contoursDetected = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(input, contoursDetected, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
            return Result.Ok(contoursDetected.ToArrayOfArray());
        }
    }
}
