namespace RISK.Application.GamePlaying
{
    public class TerritoriesFactory : ITerritoriesFactory
    {
        public Territories Create()
        {
            return new Territories();
        }
    }
}