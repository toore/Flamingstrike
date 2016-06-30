using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface IAttacker
    {
        bool CanAttack(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion defendingRegion);
        AttackOutcome Attack(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion defendingRegion);
    }

    public class Attacker : IAttacker
    {
        private readonly IBattle _battle;

        public Attacker(IBattle battle)
        {
            _battle = battle;
        }

        public bool CanAttack(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion defendingRegion)
        {
            var attackingTerritory = territories.Single(x => x.Region == attackingRegion);
            var defendingTerritory = territories.Single(x => x.Region == defendingRegion);

            var canAttack =
                HasBorder(attackingTerritory, defendingTerritory)
                &&
                IsAttackerAndDefenderDifferentPlayers(attackingTerritory, defendingTerritory)
                &&
                HasAttackerEnoughArmiesToPerformAttack(attackingTerritory);

            return canAttack;
        }

        private static bool HasBorder(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Region.HasBorder(defendingTerritory.Region);
        }

        private static bool IsAttackerAndDefenderDifferentPlayers(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Player != defendingTerritory.Player;
        }

        private static bool HasAttackerEnoughArmiesToPerformAttack(ITerritory attackingTerritory)
        {
            return attackingTerritory.GetNumberOfArmiesAvailableForAttack() > 0;
        }

        public AttackOutcome Attack(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion defendingRegion)
        {
            if (!CanAttack(territories, attackingRegion, defendingRegion))
            {
                throw new InvalidOperationException("Can't attack");
            }

            var attackingTerritory = territories.Single(territory => territory.Region == attackingRegion);
            var defendingTerritory = territories.Single(territory => territory.Region == defendingRegion);

            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            var updatedTerritories = territories
                .Replace(attackingTerritory, battleResult.UpdatedAttackingTerritory)
                .Replace(defendingTerritory, battleResult.UpdatedDefendingTerritory)
                .ToList();

            var attackOutcome = battleResult.IsDefenderDefeated() ? DefendingArmy.IsEliminated
                : DefendingArmy.IsNotEliminated;

            return new AttackOutcome(updatedTerritories, attackOutcome);
        }
    }

    public enum DefendingArmy
    {
        IsNotEliminated,
        IsEliminated
    }

    public class AttackOutcome
    {
        public IReadOnlyList<ITerritory> Territories { get; }
        public DefendingArmy DefendingArmy { get; }

        public AttackOutcome(IReadOnlyList<ITerritory> territories, DefendingArmy defendingArmy)
        {
            Territories = territories;
            DefendingArmy = defendingArmy;
        }
    }
}