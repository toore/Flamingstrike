using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameboardRules
    {
        IEnumerable<ITerritory> GetAttackeeCandidates(IList<GameboardTerritory> gameboardTerritories, ITerritory attackingTerritory);
    }

    public class GameboardRules : IGameboardRules
    {
        public IEnumerable<ITerritory> GetAttackeeCandidates(IList<GameboardTerritory> gameboardTerritories, ITerritory attackingTerritory)
        {
            var attacker = gameboardTerritories
                .Single(x => x.Territory == attackingTerritory);

            var attackCandidates = gameboardTerritories
                .Where(attackee => CanAttack(attacker, attackee))
                .Select(x => x.Territory)
                .ToList();

            return attackCandidates;
        }

        private static bool CanAttack(IGameboardTerritory attacker, IGameboardTerritory attackee)
        {
            var hasBorder = attacker.Territory.HasBorderTo(attackee.Territory);
            var attackerAndAttackeeIsDifferentPlayers = attacker.Player != attackee.Player;
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