namespace vrpr.Core.Infrastructure
{
    public interface IProcessor<in TIn, out TOut> where TOut: Result
    {
        TOut Process(TIn input);
    }
}
