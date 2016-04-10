namespace vrpr.Core
{
    public interface IProcessor<in TIn, out TOut> where TOut: Result
    {
        TOut Process(TIn input);
    }
}
