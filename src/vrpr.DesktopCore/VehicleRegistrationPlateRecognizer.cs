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
        private readonly Func<Process<byte[]>> _processFactory;

        public VehicleRegistrationPlateRecognizer(Func<Process<byte[]>> processFactory)
        {
            _processFactory = processFactory;
        }

        public Result<IEnumerable<string>> Process(byte[] input)
        {
            Mat binarizedPlate = null;

            return _processFactory.Invoke()
                .Use(input)
                .Do<ReadImageFromBytesProcessor, Mat>()
                .Do<MakeImageGrayProcessor, Mat>()
                .Do<DetectAndCropPlateNumberProcessor, IEnumerable<Mat>>()
                .ForEachItem(p => 
                    p.Do<LogCurrentImageProcessor, Mat>()
                    .SaveCurrentResultTo(r => binarizedPlate = r.Value.Clone())
                    .Do<BinarizationProcessor, Mat>()
                    //.Do<GaussianBlurProcessor, Mat>()
                    //.Do<CannyProcessor, Mat>()
                    .Do<FindContoursProcessor, Point[][]>()
                    .Do<SelectLettersContours, Point[][]>(processor => processor.UseImage(binarizedPlate))
                    .Do<CropLettersProcessor, IEnumerable<Mat>>(processor => processor.UseImage(binarizedPlate))
                    .ForEachItem(ip => ip.Do<TeseractOcrProcessor, char>()))
                .Do<StringAgregateProcessor, IEnumerable<string>>()
                .GetResult();
        }
    }
}
