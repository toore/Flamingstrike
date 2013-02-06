using Caliburn.Micro;
using GuiWpf.Views.Main;
using GuiWpf.Views.WorldMapView;

namespace GuiWpf.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel(IGameEngine gameEngine)
        {
            //var gameEngine = ObjectFactory.GetInstance<IGameEngine>();
            //var worldMapViewModel = gameEngine.GetWorldMapViewModel();

            WorldMapViewModel = gameEngine.GetWorldMapViewModel();
        }

        public WorldMapViewModel WorldMapViewModel { get; set; }
    }
}