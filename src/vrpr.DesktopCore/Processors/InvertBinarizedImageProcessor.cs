using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class InvertBinarizedImageProcessor : IProcessor<Mat, Mat>
    {
        public Result<Mat> Process(Mat input)
        {
            CvInvoke.Threshold(input, input, 0, 255, ThresholdType.BinaryInv);
            return Result<Mat>.Ok(input);
        }
    }
}
