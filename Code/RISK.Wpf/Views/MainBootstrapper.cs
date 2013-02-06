using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using StructureMap;

namespace GuiWpf.Views
{
    public class MainBootstrapper : Bootstrapper<MainViewModel>
    {
        protected override void Configure()
        {
            PluginConfiguration.Configure();
        }

        protected override void BuildUp(object instance)
        {
            ObjectFactory.BuildUp(instance);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (key == null)
            {
                return ObjectFactory.GetInstance(service);
            }
            
            return ObjectFactory.Container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return ObjectFactory.GetAllInstances(service).Cast<object>();
        }
    }
}