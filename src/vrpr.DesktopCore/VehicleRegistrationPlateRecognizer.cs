using System;
using System.Collections.Generic;
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
            return _processFactory.Invoke()
                .Use(input)
                .Do<ReadImageFromBytesProcessor, Mat>()
                .Do<MakeImageGrayProcessor, Mat>()
                .Do<DetectAndCropPlateNumberProcessor, IEnumerable<Mat>>()
                .ForEachItem(p => 
                    p.Do<LogCurrentImageProcessor, Mat>()
                    .Do<FindLettersProcessor, IEnumerable<Mat>>()
                    .ForEachItem(ip => ip.Do<TeseractOcrProcessor, string>()))
                .Do<StringAgregateProcessor, IEnumerable<string>>()
                .GetResult();
        }
    }
}
