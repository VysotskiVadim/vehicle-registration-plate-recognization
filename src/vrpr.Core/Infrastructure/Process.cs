using System.Collections.Generic;
using System.Linq;

namespace vrpr.Core.Infrastructure
{
    public class Process<T>
    {
        protected Result<T> Obj { get; set; }

        public Process(Result<T> result)
        {
            Obj = result;
        } 

        public Process<TOut> Then<TOut>(IProcessor<T, TOut> processor)
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

    public class MultiItemProcess<T> : Process<IEnumerable<T>>
    {
        public MultiItemProcess(Result<IEnumerable<T>> result) : base(result)
        {
        }

        public MultiItemProcess<TOut> ThenForEach<TOut>(IProcessor<T, TOut> processor)
        {
            MultiItemProcess<TOut> result;
            if (Obj.Success)
            {
                var results = Obj.Value.Select(processor.Process).Where(r => r.Success).Select(r => r.Value).ToList();
                if (results.Any())
                {
                    result = new MultiItemProcess<TOut>(Result.Ok<IEnumerable<TOut>>(results));
                }
                else
                {
                    result = new MultiItemProcess<TOut>(Result.Fail<IEnumerable<TOut>>("No one processors return a Success result"));
                }
            }
            else
            {
                result = new MultiItemProcess<TOut>(Result.Fail<IEnumerable<TOut>>(Obj.Error));
            }

            return result;
        }
    }

    public static class ProcessHelper
    {
        public static Process<TOut> Process<TIn, TOut>(this TIn input, IProcessor<TIn, TOut> processor)
        {
            return new Process<TIn>(Result<TIn>.Ok(input)).Then(processor);
        }
    }
}
