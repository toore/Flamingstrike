using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class WorldMap : IWorldMap
    {
        private readonly List<Territory> _territories;

        public WorldMap(ITerritoryLocationRepository territoryLocationRepository)
        {
            _territories = territoryLocationRepository.GetAll()
                .Select(x => new Territory(x))
                .ToList();
        }

        public ITerritory GetTerritory(ITerritoryLocation territoryLocation)
        {
            return _territories.Single(x => x.TerritoryLocation == territoryLocation);
        }
    }
}