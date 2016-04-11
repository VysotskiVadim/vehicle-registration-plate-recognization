﻿using Microsoft.Practices.Unity;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Unity;
using vrpr.DesktopCore;

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