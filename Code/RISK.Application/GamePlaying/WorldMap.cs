using System.Collections.Generic;
using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class WorldMap : IWorldMap
    {
        private readonly List<Territory> _territories;

        public WorldMap(Locations locations)
        {
            _territories = locations.GetAll()
                .Select(x => new Territory(x))
                .ToList();
        }

        public ITerritory GetTerritory(ILocation location)
        {
            return _territories.Single(x => x.Location == location);
        }

        public IEnumerable<ITerritory> GetTerritoriesOccupiedBy(IPlayer player)
        {
            return _territories
                .Where(x => x.Occupant == player)
                .ToList();
        }

        public IEnumerable<IPlayer> GetAllPlayersOccupyingTerritories()
        {
            return _territories
                .Where(x => x.IsOccupied())
                .Select(x => x.Occupant)
                .Distinct()
                .ToList();
        }
    }
}