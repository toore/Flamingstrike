using System;
using System.Collections.Generic;
using System.Linq;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
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

                    x.For<ILocationProvider>().Singleton();

                    x.RegisterInterceptor(new HandleInterceptor<IGameSetupEventAggregator>());
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