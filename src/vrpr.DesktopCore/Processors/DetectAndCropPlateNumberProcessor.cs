using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.Practices.ObjectBuilder2;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class DetectAndCropPlateNumberProcessor : IProcessor<Mat, IEnumerable<Mat>>
    {
        private readonly IDebugLogBuilder _debugLogBuilder;

        public DetectAndCropPlateNumberProcessor(IDebugLogBuilder debugLogBuilder)
        {
            _debugLogBuilder = debugLogBuilder;
        }

        public Result<IEnumerable<Mat>> Process(Mat input)
        {
            return Result.Ok(DetectAndCrop(input));
        }

        public IEnumerable<Mat> DetectAndCrop(Mat input)
        {
            var numberCascade = new CascadeClassifier("haarcascade_russian_plate_number.xml");
            var rectangles = numberCascade.DetectMultiScale(input);

            _debugLogBuilder.AddMessage($"Haarcascade found {rectangles.Length}");
            var inputCoppy = input.Clone();
            rectangles.ForEach(r => CvInvoke.Rectangle(inputCoppy, r, new MCvScalar(255, 0, 0), 3, LineType.FourConnected));
            _debugLogBuilder.AddImage(inputCoppy);

            return rectangles.Select(rectangle => new Mat(input, rectangle)).ToList();
        }
    }
}
