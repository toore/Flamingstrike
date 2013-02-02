using System.Collections.Generic;
using GuiWpf.Views.WorldMapView.Territories;

namespace GuiWpf.Views.WorldMapView
{
    public class WorldMapViewModel
    {
        public IEnumerable<TerritoryViewModelBase> Territories { get; set; }
    }
}