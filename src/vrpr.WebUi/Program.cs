using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Unity;
using vrpr.DesktopCore;

namespace vrpr.WebUi
{
    class Program
    {
        static void Main(string[] args)
        {
            var nancyHost = new Nancy.Hosting.Self.NancyHost(new Uri("http://localhost:9664"));
            nancyHost.Start();
            Console.WriteLine("Web server running...");

            Console.ReadLine();
            nancyHost.Stop();
        }

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

        public class Bootstrapper : UnityNancyBootstrapper
        {
            protected override void ApplicationStartup(IUnityContainer container, IPipelines pipelines)
            {
                // No registrations should be performed in here, however you may
                // resolve things that are needed during application startup.
            }

            protected override void ConfigureApplicationContainer(IUnityContainer existingContainer)
            {
                // Perform registation that should have an application lifetime
                existingContainer.RegisterInstance<IUnityContainer>(existingContainer);
            }

            protected override void ConfigureRequestContainer(IUnityContainer container, NancyContext context)
            {
                // Perform registrations that should have a request lifetime
                container.RegisterType<IVehicleRegistrationPlateRecognizer, VehicleRegistrationPlateRecognizer>();
            }

            protected override void RequestStartup(IUnityContainer container, IPipelines pipelines, NancyContext context)
            {
                // No registrations should be performed in here, however you may
                // resolve things that are needed during request startup.
            }
        }
    }
}
