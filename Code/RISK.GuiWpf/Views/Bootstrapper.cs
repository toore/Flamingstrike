using System.Windows;
using Caliburn.Micro;
using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainGameViewModel>();
        }
    }
}