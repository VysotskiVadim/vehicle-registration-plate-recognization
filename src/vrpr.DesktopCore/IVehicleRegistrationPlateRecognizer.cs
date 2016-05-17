using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore
{
    public interface IVehicleRegistrationPlateRecognizer : IFilter<byte[], IEnumerable<string>>
    {
    }
}
