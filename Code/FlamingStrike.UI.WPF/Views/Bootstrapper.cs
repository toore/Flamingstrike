using System.Windows;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.ViewModels;

namespace FlamingStrike.UI.WPF.Views
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