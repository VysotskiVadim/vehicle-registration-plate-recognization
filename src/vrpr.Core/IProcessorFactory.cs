using vrpr.Core.Infrastructure;

namespace vrpr.Core
{
    public interface IProcessorFactory
    {
        TProcessor GetProcessor<TProcessor>() where TProcessor : IProcessor;
    }
}
