using Emgu.CV;
using Emgu.CV.CvEnum;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class ReadImageFromBytesProcessor : IProcessor<byte[], Mat>
    {
        private readonly IDebugLogger _debugLogger;

        public ReadImageFromBytesProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<Mat> Process(byte[] input)
        {
            var image = new Mat();
            CvInvoke.Imdecode(input, LoadImageType.AnyColor, image);
            _debugLogger.Log(debugLogBuilder => debugLogBuilder.AddMessage("Start process image").AddImage(image));
            return Result<Mat>.Ok(image);
        }
    }
}
