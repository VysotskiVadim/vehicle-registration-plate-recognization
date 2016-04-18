using System;
using System.Collections.Generic;
using System.Linq;

namespace vrpr.Core.Infrastructure
{
    public class Process<T>
    {
        internal IProcessorFactory ProcessorFactory { get; }

        public Process(IProcessorFactory processorFactory)
        {
            ProcessorFactory = processorFactory;
        }

        protected Result<T> Obj { get; set; }

        public Process<TOut> Do<TProcessor, TOut>(Action<TProcessor> initAction) where TProcessor: IProcessor<T, TOut>
        {
            Process<TOut> result;
            if (Obj.Success)
            {
                var processor = ProcessorFactory.GetProcessor<TProcessor>();
                initAction(processor);
                var processingResult = processor.Process(Obj.Value);
                result = new Process<TOut>(ProcessorFactory).Use(processingResult);
            }
            else
            {
                result = new Process<TOut>(ProcessorFactory).Use(Result.Fail<TOut>(Obj.Error));
            }

            return result;
        }

        public Process<TOut> Do<TProcessor, TOut>() where TProcessor : IProcessor<T, TOut>
        {
            Process<TOut> result;
            if (Obj.Success)
            {
                var processor = ProcessorFactory.GetProcessor<TProcessor>();
                var processingResult = processor.Process(Obj.Value);
                result = new Process<TOut>(ProcessorFactory).Use(processingResult);
            }
            else
            {
                result = new Process<TOut>(ProcessorFactory).Use(Result.Fail<TOut>(Obj.Error));
            }

            return result;
        }

        public Result<T> GetResult()
        {
            return Obj;
        }

        public Process<T> Use(T obj)
        {
            Obj = Result<T>.Ok(obj);
            return this;
        }

        public Process<T> Use(Result<T> obj)
        {
            Obj = obj;
            return this;
        }

        public Process<T> SaveCurrentResultTo(out Result<T> storage)
        {
            storage = this.GetResult();
            return this;
        } 
    }

    public static class ProcessorHelper
    {
        public static Process<IEnumerable<TOut>> ForEachItem<T, TOut>(this Process<IEnumerable<T>> process, Func<Process<T>, Process<TOut>>  action)
        {
            var currentResult = process.GetResult();
            var result = new List<TOut>();
            if (currentResult.Success)
            {
                foreach (var item in currentResult.Value)
                {
                    var processResult = action(new Process<T>(process.ProcessorFactory).Use(item)).GetResult();
                    if (processResult.Success)
                    {
                        result.Add(processResult.Value);
                    }
                }
            }

            if (result.Any())
            {
                return new Process<IEnumerable<TOut>>(process.ProcessorFactory).Use(Result.Ok(result.AsEnumerable()));
            }
            else
            {
                return new Process<IEnumerable<TOut>>(process.ProcessorFactory).Use(Result.Fail<IEnumerable<TOut>>("Not one processor hasn't finish successfulley"));
            }
        }
    }
}
