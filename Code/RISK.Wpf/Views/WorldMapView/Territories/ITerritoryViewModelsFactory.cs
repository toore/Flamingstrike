using System.Collections.Generic;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public interface ITerritoryViewModelsFactory
    {
        IEnumerable<TerritoryViewModelBase> Create();
    }
}