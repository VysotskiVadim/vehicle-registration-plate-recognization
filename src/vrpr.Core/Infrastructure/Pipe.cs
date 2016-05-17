using System;
using System.Collections.Generic;
using System.Linq;

namespace vrpr.Core.Infrastructure
{
    public class Pipe<T>
    {
        internal IPipeFactory PipeFactory { get; }

        public Pipe(IPipeFactory pipeFactory)
        {
            PipeFactory = pipeFactory;
        }

        protected Result<T> Obj { get; set; }

        public Pipe<TOut> Do<TFilter, TOut>(Action<TFilter> initAction) where TFilter: IFilter<T, TOut>
        {
            Pipe<TOut> result;
            if (Obj.Success)
            {
                var processor = PipeFactory.GetProcessor<TFilter>();
                initAction(processor);
                var processingResult = processor.Process(Obj.Value);
                result = new Pipe<TOut>(PipeFactory).Use(processingResult);
            }
            else
            {
                result = new Pipe<TOut>(PipeFactory).Use(Result.Fail<TOut>(Obj.Error));
            }

            return result;
        }

        public Pipe<TOut> Do<TProcessor, TOut>() where TProcessor : IFilter<T, TOut>
        {
            Pipe<TOut> result;
            if (Obj.Success)
            {
                var processor = PipeFactory.GetProcessor<TProcessor>();
                var processingResult = processor.Process(Obj.Value);
                result = new Pipe<TOut>(PipeFactory).Use(processingResult);
            }
            else
            {
                result = new Pipe<TOut>(PipeFactory).Use(Result.Fail<TOut>(Obj.Error));
            }

            return result;
        }

        public Result<T> GetResult()
        {
            return Obj;
        }

        public Pipe<T> Use(T obj)
        {
            Obj = Result<T>.Ok(obj);
            return this;
        }

        public Pipe<T> Use(Result<T> obj)
        {
            Obj = obj;
            return this;
        }

        public Pipe<T> SaveCurrentResultTo(out Result<T> storage)
        {
            storage = this.GetResult();
            return this;
        }

        public Pipe<T> SaveCurrentResultTo(Action<Result<T>> saveCurrentResultAction)
        {
            saveCurrentResultAction(this.GetResult());
            return this;
        }
    }

    public static class PipeEmumerateHelper
    {
        public static Pipe<IEnumerable<TOut>> ForEachItem<T, TOut>(this Pipe<IEnumerable<T>> pipe, Func<Pipe<T>, Pipe<TOut>>  action)
        {
            var currentResult = pipe.GetResult();
            var result = new List<TOut>();
            if (currentResult.Success)
            {
                foreach (var item in currentResult.Value)
                {
                    var processResult = action(new Pipe<T>(pipe.PipeFactory).Use(item)).GetResult();
                    if (processResult.Success)
                    {
                        result.Add(processResult.Value);
                    }
                }
            }

            if (result.Any())
            {
                return new Pipe<IEnumerable<TOut>>(pipe.PipeFactory).Use(Result.Ok(result.AsEnumerable()));
            }
            else
            {
                return new Pipe<IEnumerable<TOut>>(pipe.PipeFactory).Use(Result.Fail<IEnumerable<TOut>>("Not one processor hasn't finish successfulley"));
            }
        }
    }
}
