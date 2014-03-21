namespace RISK.Domain.GamePlaying
{
    public class WorldMapFactory : IWorldMapFactory
    {
        private readonly Locations _locations;

        public WorldMapFactory(Locations locations)
        {
            _locations = locations;
        }

        public IWorldMap Create()
        {
            return new WorldMap(_locations);
        }
    }
}