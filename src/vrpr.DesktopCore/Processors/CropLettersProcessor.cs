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

            var imageToCrop = _image;
            List<Mat> result = new List<Mat>();
            foreach (var letterCountor in input)
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

                rectSize.Width += rectSize.Width*0.1f;
                rectSize.Height += rectSize.Height*0.1f;

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
