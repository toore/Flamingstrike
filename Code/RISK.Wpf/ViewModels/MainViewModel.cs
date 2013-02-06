using Caliburn.Micro;
using GuiWpf.Views.WorldMap;

namespace GuiWpf.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        public MainViewModel(IGameEngine gameEngine)
        {
            WorldMapViewModel = gameEngine.GetWorldMapViewModel();
        }

        public WorldMapViewModel WorldMapViewModel { get; set; }
    }
}