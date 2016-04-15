namespace vrpr.Core.Infrastructure
{
    public interface IProcessor
    {
    }

    public interface IProcessor<in TIn, TOut> : IProcessor
    {
        Result<TOut> Process(TIn input);
    }
}
