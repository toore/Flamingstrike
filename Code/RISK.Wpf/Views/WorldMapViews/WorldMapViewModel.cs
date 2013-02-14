using System.Collections.ObjectModel;
using GuiWpf.ViewModels.Gameboard.WorldMap;

namespace GuiWpf.Views.WorldMapViews
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