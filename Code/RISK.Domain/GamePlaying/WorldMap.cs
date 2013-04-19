using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class WorldMap : IWorldMap
    {
        private readonly List<Territory> _territories;

        public WorldMap(ILocationProvider locationProvider)
        {
            _territories = locationProvider.GetAll()
                .Select(x => new Territory(x))
                .ToList();
        }

        public ITerritory GetTerritory(ILocation location)
        {
            return _territories.Single(x => x.Location == location);
        }
    }
}