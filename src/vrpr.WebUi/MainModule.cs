using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.Unity;
using Nancy;
using vrpr.DesktopCore;

namespace vrpr.WebUi
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            Get["/"] = p => View["selectFilesForm.html"];
            Post["/"] = p =>
            {
                var existingContainer = new UnityContainer();
                existingContainer.RegisterInstance<IUnityContainer>(existingContainer);
                existingContainer.RegisterType<IVehicleRegistrationPlateRecognizer, VehicleRegistrationPlateRecognizer>();

                var recognizer = existingContainer.Resolve<IVehicleRegistrationPlateRecognizer>();
                var numbers = new List<string>();
                foreach (var file in Request.Files)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.Value.CopyTo(ms);
                        var imageData = ms.ToArray();
                        var recognizeResult = recognizer.Process(imageData);
                        if (recognizeResult.Success)
                        {
                            numbers.AddRange(recognizeResult.Value);
                        }
                    }
                }
                return string.Join(", ", numbers);
            };
        }
    }
}
