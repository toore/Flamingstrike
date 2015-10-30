using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameRules
    {
        IEnumerable<ITerritoryId> GetAttackeeCandidates(ITerritoryId attackingTerritoryId, IReadOnlyList<ITerritory> territories);
    }

    public class GameRules : IGameRules
    {
        public IEnumerable<ITerritoryId> GetAttackeeCandidates(ITerritoryId attackingTerritoryId, IReadOnlyList<ITerritory> territories)
        {
            var attacker = territories
                .Single(x => x.TerritoryId == attackingTerritoryId);

            var attackCandidates = territories
                .Where(attackee => CanAttack(attacker, attackee))
                .Select(x => x.TerritoryId)
                .ToList();

            return attackCandidates;
        }

        private static bool CanAttack(ITerritory attacker, ITerritory attackee)
        {
            var hasBorder = attacker.TerritoryId.HasBorderTo(attackee.TerritoryId);
            var attackerAndAttackeeIsDifferentPlayers = attacker.PlayerId != attackee.PlayerId;
            var hasEnoughArmiesToAttack = attacker.GetNumberOfAttackingArmies() > 0;

            var canAttack = hasBorder
                            &&
                            attackerAndAttackeeIsDifferentPlayers
                            &&
                            hasEnoughArmiesToAttack;

            return canAttack;
        }
    }
}