﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using vrpr.Core;
using vrpr.Core.Infrastructure;

namespace vrpr.WebUi
{
    public class PipeFactory : IPipeFactory
    {
        private readonly IUnityContainer _unityContainer;

        public PipeFactory(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public TProcessor GetProcessor<TProcessor>() where TProcessor : IProcessor
        {
            return _unityContainer.Resolve<TProcessor>();
        }
    }
}
