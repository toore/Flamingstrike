using RISK.Application.World;

namespace RISK.Application
{
    public interface ITerritory
    {
        ITerritoryId TerritoryId { get; }
        IPlayerId PlayerId { get; }
        int Armies { get; }
    }

    public class Territory : ITerritory
    {
        public Territory(ITerritoryId territoryId, IPlayerId playerId, int initialArmy)
        {
            TerritoryId = territoryId;
            PlayerId = playerId;
            Armies = initialArmy;
        }

        public ITerritoryId TerritoryId { get; }
        public IPlayerId PlayerId { get; }
        public int Armies { get; set; }
    }
}