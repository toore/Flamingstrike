using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.ViewModels.Gameplay;
using RISK.Application;
using RISK.Application.GamePlaying;
using StructureMap;

namespace GuiWpf.Infrastructure
{
    public class PluginConfiguration
    {
        public void Configure()
        {
            ObjectFactory.Configure(x =>
            {
                x.Scan(s =>
                {
                    s.AssemblyContainingType<IGame>();
                    s.AssemblyContainingType<IGameboardViewModel>();

                    s.WithDefaultConventions();
                });

                x.For<RISK.Application.Territories>().Singleton();

                x.For<Players>().Singleton();
                x.For<IPlayers>().Singleton().Use<Players>();
                x.Forward<IPlayers, IPlayersInitializer>();

                x.For<IEventAggregator>().Use<EventAggregator>();

                x.RegisterInterceptor(new HandleInterceptor<IEventAggregator>());
            });
        }

        public void BuildUp(object target)
        {
            ObjectFactory.BuildUp(target);
        }

        public object GetInstance(Type pluginType, string instanceKey)
        {
            if (instanceKey == null)
            {
                return ObjectFactory.GetInstance(pluginType);
            }

            return ObjectFactory.Container.GetInstance(pluginType, instanceKey);
        }

        public IEnumerable<object> GetAllInstances(Type pluginType)
        {
            return ObjectFactory.GetAllInstances(pluginType).Cast<object>();
        }
    }
}