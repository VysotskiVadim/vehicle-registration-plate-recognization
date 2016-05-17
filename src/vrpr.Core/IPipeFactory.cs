using vrpr.Core.Infrastructure;

namespace vrpr.Core
{
    public interface IPipeFactory
    {
        TProcessor GetProcessor<TProcessor>() where TProcessor : IProcessor;
    }
}
