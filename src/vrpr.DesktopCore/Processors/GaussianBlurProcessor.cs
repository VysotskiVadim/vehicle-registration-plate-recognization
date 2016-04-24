using System.Drawing;
using Emgu.CV;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class GaussianBlurProcessor : IProcessor<Mat, Mat>
    {
        public Result<Mat> Process(Mat input)
        {
            var level = 3;
            CvInvoke.GaussianBlur(input, input, new Size(level, level), 0);
            return Result<Mat>.Ok(input);
        }
    }
}
