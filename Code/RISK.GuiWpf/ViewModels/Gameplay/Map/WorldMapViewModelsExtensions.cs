using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;

namespace GuiWpf.ViewModels.Gameplay.Map
{
    public static class WorldMapViewModelsExtensions
    {
        public static ITerritoryLayoutViewModel GetTerritoryLayout(this IEnumerable<IWorldMapItemViewModel> worldMapViewModels, ITerritory location)
        {
            return worldMapViewModels
                .OfType<ITerritoryLayoutViewModel>()
                .Single(x => x.Location == location);
        }

        public static ITerritoryTextViewModel GetTerritoryTextViewModel(this IEnumerable<IWorldMapItemViewModel> worldMapViewModels, ITerritory location)
        {
            return worldMapViewModels
                .OfType<ITerritoryTextViewModel>()
                .Single(x => x.Territory == location);
        } 
    }
}