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
                .Then<ReadImageFromBytesProcessor, Mat>()
                .Then<MakeImageGrayProcessor, Mat>()
                .Then<DetectAndCropPlateNumberProcessor, Mat, IEnumerable<Mat>>()
                .ThenForEach<LogCurrentImageProcessor, Mat>()
                .ThenForEach<TeseractOcrProcessor, string>()
                .GetResult();
        }
    }
}
