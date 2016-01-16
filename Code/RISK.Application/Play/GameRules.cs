using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameRules
    {
        IEnumerable<ITerritoryGeography> GetAttackeeCandidates(ITerritoryGeography attackingTerritoryGeography, IReadOnlyList<ITerritory> territories);
    }

    public class GameRules : IGameRules
    {
        public IEnumerable<ITerritoryGeography> GetAttackeeCandidates(ITerritoryGeography attackingTerritoryGeography, IReadOnlyList<ITerritory> territories)
        {
            var attacker = territories
                .Single(x => x.TerritoryGeography == attackingTerritoryGeography);

            var attackCandidates = territories
                .Where(attackee => CanAttack(attacker, attackee))
                .Select(x => x.TerritoryGeography)
                .ToList();

            return attackCandidates;
        }

        private static bool CanAttack(ITerritory attacker, ITerritory attackee)
        {
            var hasBorder = attacker.TerritoryGeography.HasBorder(attackee.TerritoryGeography);
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