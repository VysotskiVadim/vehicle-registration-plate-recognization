namespace vrpr.Core.Infrastructure
{
    public interface IProcessor<in TIn, TOut>
    {
        Result<TOut> Process(TIn input);
    }
}
