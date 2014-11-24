using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class StructureMapBootstrapper : BootstrapperBase
    {
        private readonly PluginConfiguration _pluginConfiguration = new PluginConfiguration();

        public StructureMapBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IMainGameViewModel>();
        }

        protected override void Configure()
        {
            _pluginConfiguration.Configure();
        }

        protected override void BuildUp(object instance)
        {
            _pluginConfiguration.BuildUp(instance);
        }

        protected override object GetInstance(Type service, string key)
        {
            return _pluginConfiguration.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _pluginConfiguration.GetAllInstances(service);
        }
    }
}