using System;
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
                return View["recognizationResults.html"];
            };
        }
    }
}
