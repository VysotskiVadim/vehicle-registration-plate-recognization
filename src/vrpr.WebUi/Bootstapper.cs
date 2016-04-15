using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Unity;
using vrpr.Core;
using vrpr.DesktopCore;
using vrpr.DesktopCore.DebugLog;
using vrpr.WebUi.Logger;

namespace vrpr.WebUi
{
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
            existingContainer.RegisterType<IProcessorFactory, ProcessorFactory>();
            existingContainer.RegisterType<IVehicleRegistrationPlateRecognizer, VehicleRegistrationPlateRecognizer>();
        }

        protected override void ConfigureRequestContainer(IUnityContainer container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime
#pragma warning disable 618
            container.RegisterType<IDebugLogBuilder, WebDebugLogBuilder>(new ContainerControlledLifetimeManager());
#pragma warning restore 618
            container.RegisterType<IDebugLogProvier, WebDebugLogBuilder>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDebugLogger, DebugLogger>(new ContainerControlledLifetimeManager());
        }

        protected override void RequestStartup(IUnityContainer container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
        }
    }
}
