using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IPlayerEliminationRules
    {
        bool IsPlayerEliminated(IEnumerable<ITerritory> territories, PlayerName playerName);
        bool IsOnlyOnePlayerLeftInTheGame(IEnumerable<ITerritory> territories);
    }

    public class PlayerEliminationRules : IPlayerEliminationRules
    {
        public bool IsPlayerEliminated(IEnumerable<ITerritory> territories, PlayerName playerName)
        {
            return territories.All(x => x.PlayerName != playerName);
        }

        public bool IsOnlyOnePlayerLeftInTheGame(IEnumerable<ITerritory> territories)
        {
            return territories
                .Select(x => x.PlayerName)
                .Distinct()
                .Count() == 1;
        }
    }
}