using System.Collections.ObjectModel;
using GuiWpf.ViewModels.WorldMapViewModels;

namespace GuiWpf.Views.WorldMap
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