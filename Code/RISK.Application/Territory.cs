using RISK.Application.World;

namespace RISK.Application
{
    public interface ITerritory
    {
        ITerritoryId TerritoryId { get; }
        IPlayer Player { get; }
        int Armies { get; }
    }

    public class Territory : ITerritory
    {
        public Territory(ITerritoryId territoryId, IPlayer player, int initialArmy)
        {
            TerritoryId = territoryId;
            Player = player;
            Armies = initialArmy;
        }

        public ITerritoryId TerritoryId { get; }
        public IPlayer Player { get; }
        public int Armies { get; set; }
    }
}