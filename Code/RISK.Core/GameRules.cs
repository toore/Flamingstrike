using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface IGameRules
    {
        bool IsPlayerEliminated(IEnumerable<ITerritory> territories, IPlayer player);
        bool IsGameOver(IEnumerable<ITerritory> territories);
    }

    public class GameRules : IGameRules
    {
        public bool IsPlayerEliminated(IEnumerable<ITerritory> territories, IPlayer player)
        {
            var playerOccupiesTerritories = territories.Any(x => x.Player == player);

            return !playerOccupiesTerritories;
        }

        public bool IsGameOver(IEnumerable<ITerritory> territories)
        {
            var allTerritoriesAreOccupiedBySamePlayer = territories
                .Select(x => x.Player)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }
    }
}