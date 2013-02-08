using GuiWpf.Views.WorldMap;

namespace GuiWpf.ViewModels
{
    public class MainViewModel : IMainViewModel
    {
        public MainViewModel(IGameEngine gameEngine)
        {
            WorldMapViewModel = gameEngine.WorldMapViewModel;
        }

        public WorldMapViewModel WorldMapViewModel { get; private set; }
    }
}