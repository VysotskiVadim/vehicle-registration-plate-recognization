using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class MakeImageGrayProcessor : IProcessor<Mat, Mat>
    {
        public Result<Mat> Process(Mat input)
        {
            var grayImage = new Mat();
            CvInvoke.CvtColor(input, grayImage, ColorConversion.Bgr2Gray);
            return Result.Ok(grayImage);
        }
    }
}
