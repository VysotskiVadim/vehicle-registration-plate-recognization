using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;

namespace vrpr.Core.Infrastructure
{
    public class Process<T>
    {
        protected IUnityContainer Container { get; }

        public Process(IUnityContainer unityContainer)
        {
            Container = unityContainer;
        }

        protected Result<T> Obj { get; set; }

        public Process<TOut> Then<TProcessor, TOut>() where TProcessor: IProcessor<T, TOut>
        {
            Process<TOut> result;
            if (Obj.Success)
            {
                var processor = Container.Resolve<TProcessor>();
                var processingResult = processor.Process(Obj.Value);
                result = Container.Resolve<Process<TOut>>().Use(processingResult);
            }
            else
            {
                result = Container.Resolve<Process<TOut>>().Use(Result.Fail<TOut>(Obj.Error));
            }

            return result;
        }

        public MultiItemProcess<TItem> Then<TProcessor, TItem, TOut>() where TOut : IEnumerable<TItem> where TProcessor: IProcessor<T, TOut>
        {
            MultiItemProcess<TItem> result;
            if (Obj.Success)
            {
                var processor = Container.Resolve<TProcessor>();
                var processingResult = processor.Process(Obj.Value);
                if (processingResult.Success)
                {
                    result = (MultiItemProcess<TItem>) new MultiItemProcess<TItem>(Container).Use(Result.Ok((IEnumerable<TItem>) processingResult.Value));
                }
                else
                {
                    result = (MultiItemProcess<TItem>)new MultiItemProcess<TItem>(Container).Use(Result.Fail<IEnumerable<TItem>>(processingResult.Error));
                }
            }
            else
            {
                result = (MultiItemProcess<TItem>)new MultiItemProcess<TItem>(Container).Use(Result.Fail<IEnumerable<TItem>>(Obj.Error));
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
    }

    public class MultiItemProcess<T> : Process<IEnumerable<T>>
    {
        public MultiItemProcess(IUnityContainer unityContainer) : base(unityContainer)
        {
        }

        public MultiItemProcess<TOut> ThenForEach<TProcessor, TOut>() where TProcessor: IProcessor<T, TOut>
        {
            MultiItemProcess<TOut> result;
            if (Obj.Success)
            {
                var processor = Container.Resolve<TProcessor>();
                var results = Obj.Value.Select(processor.Process).Where(r => r.Success).Select(r => r.Value).ToList();
                if (results.Any())
                {
                    result = (MultiItemProcess<TOut>) new MultiItemProcess<TOut>(Container).Use(Result.Ok<IEnumerable<TOut>>(results));
                }
                else
                {
                    result = (MultiItemProcess<TOut>)new MultiItemProcess<TOut>(Container).Use(Result.Fail<IEnumerable<TOut>>("No one processors return a Success result"));
                }
            }
            else
            {
                result = (MultiItemProcess<TOut>)new MultiItemProcess<TOut>(Container).Use(Result.Fail<IEnumerable<TOut>>(Obj.Error));
            }

            return result;
        }
    }
}
