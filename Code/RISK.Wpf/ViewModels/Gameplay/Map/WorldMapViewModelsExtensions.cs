using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public static class WorldMapViewModelsExtensions
    {
        public static ITerritoryViewModel Get(this IEnumerable<IWorldMapViewModel> worldMapViewModels, ILocation location)
        {
            return worldMapViewModels
                .OfType<ITerritoryViewModel>()
                .Single(x => x.Location == location);
        } 
    }
}