using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Attacking
{
    public interface IPlayerEliminationRules
    {
        bool IsPlayerEliminated(IEnumerable<ITerritory> territories, IPlayer player);
        bool IsOnlyOnePlayerLeftInTheGame(IEnumerable<ITerritory> territories);
    }

    public class PlayerEliminationRules : IPlayerEliminationRules
    {
        public bool IsPlayerEliminated(IEnumerable<ITerritory> territories, IPlayer player)
        {
            return territories.All(x => x.Player != player);
        }

        public bool IsOnlyOnePlayerLeftInTheGame(IEnumerable<ITerritory> territories)
        {
            return territories
                .Select(x => x.Player)
                .Distinct()
                .Count() == 1;
        }
    }
}