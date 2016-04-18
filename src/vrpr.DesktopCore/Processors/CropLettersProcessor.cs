using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class CropLettersProcessor : IProcessor<Point[][], IEnumerable<Mat>>
    {
        private readonly IDebugLogger _debugLogger;

        private Mat _image;

        public CropLettersProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<IEnumerable<Mat>> Process(Point[][] input)
        {
            if (_image == null)
            {
                return Result.Fail<IEnumerable<Mat>>("Image hasn't been set for CropLettersProcessor");
            }

            var minLetterHeight = (int)(_image.Height * 0.3);
            var maxLetterHeight = (int)(_image.Height * 0.8);
            var minLetterWidth = (int)(_image.Width*0.05);

            var letterCountors = input.Where(countor =>
            {
                var countorHeight = countor.Max(point => point.Y) - countor.Min(point => point.Y);
                var countorWidth = countor.Max(point => point.X) - countor.Min(point => point.X);
                return countorHeight > minLetterHeight && countorHeight < maxLetterHeight && countorWidth > minLetterWidth;
            }).AsParallel().WithDegreeOfParallelism(2).ToArray();

            _debugLogger.Log(debugLogBuilder =>
            {
                debugLogBuilder.AddMessage($"found letters {letterCountors.Length}");
                var inputWithSelectedLetters = _image.Clone();
                for (int i = 0; i < letterCountors.Length; i++)
                {
                    CvInvoke.DrawContours(inputWithSelectedLetters, new VectorOfVectorOfPoint(letterCountors), i, new MCvScalar(255, 255, 255));
                }
                debugLogBuilder.AddImage(inputWithSelectedLetters);
            });

            var imageToCrop = _image;
            List<Mat> result = new List<Mat>();
            foreach (var letterCountor in letterCountors)
            {
                var rect = CvInvoke.MinAreaRect(letterCountor.Select<Point, PointF>(p => p).ToArray());
                float angle = rect.Angle;
                var rectSize = rect.Size;
                if (rect.Angle < -45)
                {
                    angle += 90;
                    var temp = rectSize.Width;
                    rectSize.Width = rectSize.Height;
                    rectSize.Height = temp;
                }
                var rotationMatrix = new Mat();
                CvInvoke.GetRotationMatrix2D(rect.Center, angle, 1, rotationMatrix);
                var rotated = new Mat();
                CvInvoke.WarpAffine(imageToCrop, rotated, rotationMatrix, imageToCrop.Size, Inter.Cubic);
                // crop the resulting image
                var cropped = new Mat();
                CvInvoke.GetRectSubPix(rotated, new Size((int)rectSize.Width, (int)rectSize.Height), rect.Center, cropped);
                result.Add(cropped);
                //_debugLogger.Log(logBuilder => logBuilder.AddMessage("crop letter").AddImage(cropped));
            }

            

            return Result.Ok(result.AsEnumerable());
        }

        public void UseImage(Mat image)
        {
            _image = image;
        }
    }
}
