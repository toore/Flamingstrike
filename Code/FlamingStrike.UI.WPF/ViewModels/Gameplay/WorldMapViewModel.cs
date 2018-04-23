using System.Collections.ObjectModel;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public class WorldMapViewModel
    {
        public WorldMapViewModel()
        {
            WorldMapViewModels = new ObservableCollection<IWorldMapItemViewModel>();
        }

        public ObservableCollection<IWorldMapItemViewModel> WorldMapViewModels { get; }
    }
}