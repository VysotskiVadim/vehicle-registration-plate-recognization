using System;
using System.Collections.Generic;
using System.IO;
using Nancy;
using vrpr.DesktopCore;

namespace vrpr.WebUi
{
    public class MainModule : NancyModule
    {
        public MainModule(Func<IVehicleRegistrationPlateRecognizer> recognizerFactory)
        {
            Get["/"] = p => View["selectFilesForm.html"];
            Post["/"] = p =>
            {
                var recognizer = recognizerFactory.Invoke();
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
