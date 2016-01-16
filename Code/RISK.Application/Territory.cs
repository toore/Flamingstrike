using RISK.Application.World;

namespace RISK.Application
{
    public interface ITerritory
    {
        ITerritoryGeography TerritoryGeography { get; }
        IPlayer Player { get; }
        int Armies { get; }
    }

    public class Territory : ITerritory
    {
        public Territory(ITerritoryGeography territoryGeography, IPlayer player, int initialArmy)
        {
            TerritoryGeography = territoryGeography;
            Player = player;
            Armies = initialArmy;
        }

        public ITerritoryGeography TerritoryGeography { get; }
        public IPlayer Player { get; }
        public int Armies { get; set; }
    }
}