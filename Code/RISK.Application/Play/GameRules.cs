using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameRules
    {
        IEnumerable<IRegion> GetAttackeeCandidates(IRegion attackingRegion, IReadOnlyList<ITerritory> territories);
    }

    public class GameRules : IGameRules
    {
        public IEnumerable<IRegion> GetAttackeeCandidates(IRegion attackingRegion, IReadOnlyList<ITerritory> territories)
        {
            var attacker = territories
                .Single(x => x.Region == attackingRegion);

            var attackCandidates = territories
                .Where(attackee => CanAttack(attacker, attackee))
                .Select(x => x.Region)
                .ToList();

            return attackCandidates;
        }

        private static bool CanAttack(ITerritory attacker, ITerritory attackee)
        {
            var hasBorder = attacker.Region.HasBorder(attackee.Region);
            var attackerAndAttackeeIsDifferentPlayers = attacker.Player != attackee.Player;
            var hasEnoughArmiesToAttack = attacker.GetNumberOfArmiesAvailableForAttack() > 0;

            var canAttack = hasBorder
                            &&
                            attackerAndAttackeeIsDifferentPlayers
                            &&
                            hasEnoughArmiesToAttack;

            return canAttack;
        }
    }
}