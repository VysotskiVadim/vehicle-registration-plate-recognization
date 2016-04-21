using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class MakeImageGrayProcessor : IProcessor<Mat, Mat>
    {
        public Result<Mat> Process(Mat input)
        {
            CvInvoke.CvtColor(input, input, ColorConversion.Bgr2Gray);
            return Result.Ok(input);
        }
    }
}
