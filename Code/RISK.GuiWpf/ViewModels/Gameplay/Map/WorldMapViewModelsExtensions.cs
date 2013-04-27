using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public static class WorldMapViewModelsExtensions
    {
        public static ITerritoryLayoutViewModel GetTerritoryLayout(this IEnumerable<IWorldMapViewModel> worldMapViewModels, ILocation location)
        {
            return worldMapViewModels
                .OfType<ITerritoryLayoutViewModel>()
                .Single(x => x.Location == location);
        }

        public static ITerritoryTextViewModel GetTerritoryData(this IEnumerable<IWorldMapViewModel> worldMapViewModels, ILocation location)
        {
            return worldMapViewModels
                .OfType<ITerritoryTextViewModel>()
                .Single(x => x.Location == location);
        } 
    }
}