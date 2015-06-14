using RISK.Application.World;

namespace RISK.Application
{
    public class GameboardTerritory
    {
        public GameboardTerritory(ITerritory territory, IPlayerId playerId, int armies)
        {
            Territory = territory;
            PlayerId = playerId;
            Armies = armies;
        }

        public ITerritory Territory { get; }
        public IPlayerId PlayerId { get; }
        public int Armies { get; private set; }
    }
}