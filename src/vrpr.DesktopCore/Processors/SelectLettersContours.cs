using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class SelectLettersContours : IProcessor<Point[][], Point[][]>
    {
        private Mat _image;

        public Result<Point[][]> Process(Point[][] input)
        {
            var minLetterHeight = (int)(_image.Height * 0.30);
            var maxLetterHeight = (int)(_image.Height * 0.8);
            var minLetterWidth = (int)(_image.Width * 0.04);
            var maxLetterWidth = (int)(_image.Width * 0.1);

            var letterCountors = input.Where(countor =>
            {
                var countorHeight = countor.Max(point => point.Y) - countor.Min(point => point.Y);
                var countorWidth = countor.Max(point => point.X) - countor.Min(point => point.X);
                return countorHeight > minLetterHeight && countorHeight < maxLetterHeight && countorWidth > minLetterWidth && countorWidth < maxLetterWidth;
            }).OrderBy(points => points.Min(point => point.X)).AsParallel().ToArray();

            return Result.Ok(letterCountors);
        }

        public void UseImage(Mat value)
        {
            _image = value;
        }
    }
}
