using System.IO;
using Nancy;
using vrpr.DesktopCore;
using vrpr.WebUi.Logger;

namespace vrpr.WebUi
{
    public class DevelperModule : NancyModule
    {
        public DevelperModule(IVehicleRegistrationPlateRecognizer recognizer, IDebugLogProvier logProvider)
        {
            Get["/"] = p => View["selectFilesForm.html"];
            Post["/"] = p =>
            {
                foreach (var file in Request.Files)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.Value.CopyTo(ms);
                        var imageData = ms.ToArray();
                        recognizer.Process(imageData);
                    }
                }

                var logs = logProvider.GetLog();
                return View["log.html", new { Logs = logs }];
            };
        }
    }
}
