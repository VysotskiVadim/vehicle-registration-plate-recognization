using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vrpr.Core.Infrastructure;

namespace vrpr.DesktopCore.Processors
{
    public class StringAgregateProcessor : IProcessor<IEnumerable<IEnumerable<string>>, IEnumerable<string>>
    {
        public Result<IEnumerable<string>> Process(IEnumerable<IEnumerable<string>> input)
        {
            return Result.Ok(input.Select(s => string.Concat(s)));
        }
    }
}
