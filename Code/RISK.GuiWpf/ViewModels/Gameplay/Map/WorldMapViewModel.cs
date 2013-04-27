using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Gameplay.Map
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