using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Gameboard.WorldMap
{
    public class WorldMapViewModel
    {
        private readonly ObservableCollection<IWorldMapViewModel> _worldMapViewModels;

        public WorldMapViewModel()
        {
            _worldMapViewModels = new ObservableCollection<IWorldMapViewModel>();
        }

        public ObservableCollection<IWorldMapViewModel> WorldMapViewModels
        {
            get { return _worldMapViewModels; }
        }
    }
}