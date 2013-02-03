using System.Windows;
using GuiWpf.Infrastructure;
using RISK.Domain;
using StructureMap;

namespace GuiWpf.Views.Main
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            PluginConfiguration.Configure();

            var gameEngine = ObjectFactory.GetInstance<IGameEngine>();
            var worldMapViewModel = gameEngine.GetWorldMapViewModel();

            var mainWindow = new MainWindow
                {
                    DataContext = worldMapViewModel
                };

            mainWindow.Show();
        }
    }
}