using GuiWpf.Views.WorldMap;

namespace GuiWpf.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private WorldMapViewModel _worldMapViewModel;

        public MainViewModel(IGameEngine gameEngine)
        {
            WorldMapViewModel = gameEngine.WorldMapViewModel;
        }

        public void UpdateWorldMapViewModel(WorldMapViewModel worldMapViewModel)
        {
            WorldMapViewModel = worldMapViewModel;
        }

        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            set { NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = value); }
        }
    }
}