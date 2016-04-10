using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace vrpr.DesktopCore
{
    public class Registry
    {
        public void Register(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<IVehicleRegistrationPlateRecognizer, VehicleRegistrationPlateRecognizer>();
            unityContainer.RegisterInstance<IUnityContainer>(unityContainer);
        }
    }
}
