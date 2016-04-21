using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vrpr.Core.Infrastructure;
using vrpr.DesktopCore.DebugLog;

namespace vrpr.DesktopCore.Processors
{
    public class StringAgregateProcessor : IProcessor<IEnumerable<IEnumerable<char>>, IEnumerable<string>>
    {
        private readonly IDebugLogger _debugLogger;

        public StringAgregateProcessor(IDebugLogger debugLogger)
        {
            _debugLogger = debugLogger;
        }

        public Result<IEnumerable<string>> Process(IEnumerable<IEnumerable<char>> input)
        {
            return Result.Ok(input.Select(s =>
            {
                var result = string.Concat(s);
                _debugLogger.Log(logBuilder => logBuilder.AddMessage($"Founded licence plate: {result}"));
                return result;
            }).ToList().AsEnumerable());
        }
    }
}
