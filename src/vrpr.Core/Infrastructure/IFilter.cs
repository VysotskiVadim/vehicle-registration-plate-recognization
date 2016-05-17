namespace vrpr.Core.Infrastructure
{
    public interface IProcessor
    {
    }

    public interface IFilter<in TIn, TOut> : IProcessor
    {
        Result<TOut> Process(TIn input);
    }
}
