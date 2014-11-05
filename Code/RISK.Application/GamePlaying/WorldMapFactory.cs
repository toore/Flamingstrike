namespace RISK.Application.GamePlaying
{
    public interface IWorldMapFactory
    {
        IWorldMap Create();
    }

    public class WorldMapFactory : IWorldMapFactory
    {
        public IWorldMap Create()
        {
            return new WorldMap();
        }
    }
}