using System;
using System.Collections.Generic;
using Emgu.CV;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.Processors;

namespace vrpr.DesktopCore
{
    public class VehicleRegistrationPlateRecognizer : IVehicleRegistrationPlateRecognizer
    {
        private readonly Func<Process<Mat>> _processFactory;

        public VehicleRegistrationPlateRecognizer(Func<Process<Mat>> processFactory)
        {
            _processFactory = processFactory;
        }

        public Result<IEnumerable<string>> Process(Mat input)
        {
            return _processFactory.Invoke()
                .Use(input)
                .Then<MakeImageGrayProcessor, Mat>()
                .Then<DetectAndCropPlateNumberProcessor, Mat, IEnumerable<Mat>>()
                .ThenForEach<TeseractOcrProcessor, string>()
                .GetResult();
        }
    }
}
