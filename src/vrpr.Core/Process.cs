namespace vrpr.Core
{
    public class Process<T>
    {
        protected Result<T> Obj { get; set; }

        public Process(Result<T> result)
        {
            Obj = result;
        } 

        public Process<TOut> Then<TOut>(IProcessor<T, Result<TOut>> processor)
        {
            Process<TOut> result;
            if (Obj.Success)
            {
                var processingResult = processor.Process(Obj.Value);
                result = new Process<TOut>(processingResult);
            }
            else
            {
                result = new Process<TOut>(Result.Fail<TOut>(Obj.Error));
            }

            return result;
        }

        
    }

    public static class ProcessHelper
    {
        public static Process<TOut> Process<TIn, TOut>(this TIn input, IProcessor<TIn, Result<TOut>> processor)
        {
            return new Process<TIn>(Result<TIn>.Ok(input)).Then(processor);
        }
    }
}
