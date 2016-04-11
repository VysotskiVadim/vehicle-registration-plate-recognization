using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class ReadImageFromBytesProcessor : IProcessor<byte[], Mat>
    {
        public Result<Mat> Process(byte[] input)
        {
            var image = new Mat();
            CvInvoke.Imdecode(input, LoadImageType.AnyColor, image);
            return Result<Mat>.Ok(image);
        }
    }
}
