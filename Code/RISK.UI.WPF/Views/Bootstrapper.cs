using System.Windows;
using Caliburn.Micro;
using RISK.UI.WPF.ViewModels;

namespace RISK.UI.WPF.Views
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