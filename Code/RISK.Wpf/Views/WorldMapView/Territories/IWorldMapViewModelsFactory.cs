using System.Collections.Generic;

namespace GuiWpf.Views.WorldMapView.Territories
{
    public interface IWorldMapViewModelsFactory
    {
        IEnumerable<IWorldMapViewModel> Create();
    }
}