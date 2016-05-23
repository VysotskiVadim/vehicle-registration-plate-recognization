using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using vrpr.Core;

namespace vrpr.DesktopCore
{
    public class PluginLoader
    {
        public void Load(string path, IUnityContainer container)
        {
            foreach (var dlls in Directory.EnumerateFiles(path, "*.dll"))
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(dlls);
                    assembly.GetTypes()
                        .Where(mytype => mytype.GetInterfaces().Contains(typeof (IRecongnitionResultHander)))
                        .ForEach(t => container.RegisterType(typeof(IRecongnitionResultHander), t, dlls + t.FullName));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Eror to load assembly from file {e.Message}");
                    throw;
                }
            }
        }
    }
}
