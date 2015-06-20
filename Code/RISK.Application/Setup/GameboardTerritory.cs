using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface IGameboardTerritory
    {
        ITerritory Territory { get; }
        IPlayer Player { get; }
        int Armies { get; }
    }

    public class GameboardTerritory : IGameboardTerritory
    {
        public GameboardTerritory(ITerritory territory, IPlayer player, int initialArmy)
        {
            Territory = territory;
            Player = player;
            Armies = initialArmy;
        }

        public ITerritory Territory { get; }
        public IPlayer Player { get; }
        public int Armies { get; set; }
    }
}