using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IPlayerEliminationRules
    {
        bool IsPlayerEliminated(IEnumerable<ITerritory> territories, PlayerName player);
        bool IsOnePlayerLeftInTheGame(IEnumerable<ITerritory> territories);
    }

    public class PlayerEliminationRules : IPlayerEliminationRules
    {
        public bool IsPlayerEliminated(IEnumerable<ITerritory> territories, PlayerName player)
        {
            return territories.All(x => x.PlayerName != player);
        }

        public bool IsOnePlayerLeftInTheGame(IEnumerable<ITerritory> territories)
        {
            return territories
                .Select(x => x.PlayerName)
                .Distinct()
                .Count() == 1;
        }
    }
}