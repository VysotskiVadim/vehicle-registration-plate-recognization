using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.Processors;

namespace vrpr.DesktopCore
{
    public class VehicleRegistrationPlateRecognizer : IVehicleRegistrationPlateRecognizer
    {
        private readonly Func<Pipe<byte[]>> _processFactory;

        public VehicleRegistrationPlateRecognizer(Func<Pipe<byte[]>> processFactory)
        {
            _processFactory = processFactory;
        }

        public Result<IEnumerable<string>> Process(byte[] input)
        {
            Mat binarizedPlate = null;

            return _processFactory.Invoke()
                .Use(input)
                .Do<ReadImageFromBytesFilter, Mat>()
                .Do<MakeImageGrayFilter, Mat>()
                .Do<DetectAndCropPlateNumberFilter, IEnumerable<Mat>>()
                .ForEachItem(p => 
                    p.Do<LogCurrentImageFilter, Mat>()
                    .Do<OtsuBinarizationFilter, Mat>()
                    .SaveCurrentResultTo(r => binarizedPlate = r.Value.Clone())
                    .Do<InvertBinarizedImageFilter, Mat>()
                    //.Do<GaussianBlurFilter, Mat>()
                    //.Do<CannyFilter, Mat>()
                    .Do<FindContoursFilter, Point[][]>()
                    .Do<SelectLettersContours, Point[][]>(processor => processor.UseImage(binarizedPlate))
                    .Do<CropLettersFilter, IEnumerable<Mat>>(processor => processor.UseImage(binarizedPlate))
                    .ForEachItem(ip => ip.Do<GaussianBlurFilter, Mat>().Do<TeseractOcrFilter, char>()))
                .Do<StringAgregateFilter, IEnumerable<string>>()
                .GetResult();
        }
    }
}
