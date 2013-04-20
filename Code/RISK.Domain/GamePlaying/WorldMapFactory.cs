using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class WorldMapFactory : IWorldMapFactory
    {
        private readonly ILocationProvider _locationProvider;

        public WorldMapFactory(ILocationProvider locationProvider)
        {
            _locationProvider = locationProvider;
        }

        public IWorldMap Create()
        {
            return new WorldMap(_locationProvider);
        }
    }
}