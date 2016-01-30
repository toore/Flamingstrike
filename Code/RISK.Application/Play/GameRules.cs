using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Play
{
    public interface IGameRules
    {
        IEnumerable<ITerritory> GetAttackCandidates(ITerritory attacker, IEnumerable<ITerritory> territories);
    }

    public class GameRules : IGameRules
    {
        public IEnumerable<ITerritory> GetAttackCandidates(ITerritory attacker, IEnumerable<ITerritory> territories)
        {
            var attackCandidates = territories
                .Where(defender => CanAttack(attacker, defender))
                .ToList();

            return attackCandidates;
        }

        private static bool CanAttack(ITerritory attacker, ITerritory defender)
        {
            var hasBorder = attacker.Region.HasBorder(defender.Region);
            var attackerAndDefenderIsDifferentPlayers = attacker.Player != defender.Player;
            var hasEnoughArmiesToAttack = attacker.GetNumberOfArmiesAvailableForAttack() > 0;

            var canAttack = hasBorder
                            &&
                            attackerAndDefenderIsDifferentPlayers
                            &&
                            hasEnoughArmiesToAttack;

            return canAttack;
        }
    }
}