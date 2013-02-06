using System.Collections.Generic;
using GuiWpf.ViewModels.TerritoryViewModelFactories;

namespace GuiWpf.Views.WorldMap
{
    public class WorldMapViewModel
    {
        public IEnumerable<IWorldMapViewModel> WorldMapViewModels { get; set; }
    }
}