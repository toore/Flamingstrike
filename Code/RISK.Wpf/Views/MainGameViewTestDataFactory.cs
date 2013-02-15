using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using StructureMap;

namespace GuiWpf.Views
{
    public class MainGameViewTestDataFactory
    {
        private IMainGameViewModel Create()
        {
            var pluginConfiguration = new PluginConfiguration();
            pluginConfiguration.Configure();

            //var mainGameViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

            var instance = pluginConfiguration.GetInstance<IMainGameViewModel>();
            return instance;

            //return new MainGameViewModel(new GameSetupViewModelFactory(null), null, null);
        }

        public static IMainGameViewModel ViewModel
        {
            get { return new MainGameViewTestDataFactory().Create(); }
        }
    }
}