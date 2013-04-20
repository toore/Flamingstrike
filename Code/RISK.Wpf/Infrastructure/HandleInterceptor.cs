using System;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using StructureMap;
using StructureMap.Interceptors;

namespace GuiWpf.Infrastructure
{
    //public class HandleInterceptor : TypeInterceptor
    //{
    //    public object Process(object target, IContext context)
    //    {
    //        var eventAggregator = context.GetInstance<IEventAggregator>();
    //        eventAggregator.Subscribe(target);

    //        return target;
    //    }

    //    public bool MatchesType(Type type)
    //    {
    //        return type.Implements<IHandle>();
    //    }
    //}

    public class HandleInterceptor<T> : TypeInterceptor where T : IEventAggregator
    {
        public object Process(object target, IContext context)
        {
            var eventAggregator = context.GetInstance<T>();
            eventAggregator.Subscribe(target);

            return target;
        }

        public bool MatchesType(Type type)
        {
            return type.Implements<IHandle>();
        }
    }
}