using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class WorldMap : IWorldMap
    {
        private readonly List<Territory> _areas;

        public WorldMap(ITerritoryLocationRepository territoryLocationRepository)
        {
            _areas = territoryLocationRepository.GetAll()
                .Select(x => new Territory(x))
                .ToList();
        }

        public ITerritory GetArea(ITerritoryLocation territoryLocation)
        {
            return _areas.Single(x => x.TerritoryLocation == territoryLocation);
        }
    }
}