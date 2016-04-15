using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Practices.Unity;
using Nancy;
using vrpr.DesktopCore;
using vrpr.DesktopCore.DebugLog;
using vrpr.WebUi.Logger;

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
                existingContainer.RegisterType<IDebugLogBuilder, WebDebugLogBuilder>(new ContainerControlledLifetimeManager());
                existingContainer.RegisterType<IDebugLogProvier, WebDebugLogBuilder>(new ContainerControlledLifetimeManager());

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

                var logProvider = existingContainer.Resolve<IDebugLogProvier>();
                var logs = logProvider.GetLog();
                return View["log.html", new { Logs = logs }];
            };
        }
    }
}
