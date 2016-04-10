using System.Collections.Generic;
using System.Linq;
using Emgu.CV;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class DetectAndCropPlateNumberProcessor : IProcessor<Mat, IEnumerable<Mat>>
    {
        public Result<IEnumerable<Mat>> Process(Mat input)
        {
            return Result.Ok(DetectAndCrop(input));
        }

        public IEnumerable<Mat> DetectAndCrop(Mat input)
        {
            var numberCascade = new CascadeClassifier("haarcascade_russian_plate_number.xml");
            var rectangles = numberCascade.DetectMultiScale(input);
            return rectangles.Select(rectangle => new Mat(input, rectangle)).ToList();
        }
    }
}
