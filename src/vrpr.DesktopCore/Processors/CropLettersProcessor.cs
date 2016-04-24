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

            _debugLogger.Log(debugLogBuilder =>
            {
                debugLogBuilder.AddMessage($"found letters {input.Length}");
                var inputWithSelectedLetters = _image.Clone();
                for (int i = 0; i < input.Length; i++)
                {
                    CvInvoke.DrawContours(inputWithSelectedLetters, new VectorOfVectorOfPoint(input), i, new MCvScalar(255, 255, 255));
                }
                debugLogBuilder.AddImage(inputWithSelectedLetters);
            });

            var rotationAngle = GetRotationAngle(input);

            var imageToCrop = _image;
            List<Mat> result = new List<Mat>();
            foreach (var letterCountor in input)
            {
                var rect = CvInvoke.BoundingRectangle(new VectorOfPoint(letterCountor));
                float angle = rotationAngle;
                var rectSize = rect.Size;
                var rectCenter = new PointF(rect.X + rectSize.Width/2f, rect.Y + rectSize.Height/2f);
                if (angle < -45)
                {
                    angle += 90;
                    var temp = rectSize.Width;
                    rectSize.Width = rectSize.Height;
                    rectSize.Height = temp;
                }

                rectSize.Width += (int)(rectSize.Width*0.1);
                rectSize.Height += (int)(rectSize.Height*0.1);

                var rotationMatrix = new Mat();
                CvInvoke.GetRotationMatrix2D(rectCenter, angle, 1, rotationMatrix);
                var rotated = new Mat();
                CvInvoke.WarpAffine(imageToCrop, rotated, rotationMatrix, imageToCrop.Size, Inter.Cubic);
                var cropped = new Mat();
                CvInvoke.GetRectSubPix(rotated, new Size((int)rectSize.Width, (int)rectSize.Height), rectCenter, cropped);
                result.Add(cropped);
                //_debugLogger.Log(logBuilder => logBuilder.AddMessage("crop letter").AddImage(cropped));
            }

            return Result.Ok(result.AsEnumerable());
        }

        private float GetRotationAngle(Point[][] contours)
        {
            var hieghstPointsInTwoBiggestCountors =
                contours.Select(contour => contour.OrderByDescending(point => point.Y))
                    .OrderByDescending(contour => contour.First().Y)
                    .Take(2)
                    .Select(contour => contour.First())
                    .ToArray();

            var p1 = hieghstPointsInTwoBiggestCountors[0];
            var p2 = hieghstPointsInTwoBiggestCountors[1];

            var rotationAngleTangens = Math.Abs((float)p1.Y - p2.Y)/Math.Abs(p1.X - p2.X);
            return (float)(Math.Atan(rotationAngleTangens) * 180 / Math.PI);
        }

        public void UseImage(Mat image)
        {
            _image = image;
        }
    }
}
