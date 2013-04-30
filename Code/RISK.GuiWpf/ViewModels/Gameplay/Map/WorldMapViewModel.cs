using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public class WorldMapViewModel
    {
        public WorldMapViewModel()
        {
            WorldMapViewModels = new ObservableCollection<IWorldMapViewModel>();
        }

        public ObservableCollection<IWorldMapViewModel> WorldMapViewModels { get; private set; }
    }
}