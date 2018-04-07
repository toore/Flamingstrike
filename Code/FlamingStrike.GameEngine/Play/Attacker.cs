using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
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
            return attackingTerritory.PlayerName != defendingTerritory.PlayerName;
        }

        private static bool HasAttackerEnoughArmiesToPerformAttack(ITerritory attackingTerritory)
        {
            return attackingTerritory.GetNumberOfArmiesThatAreAvailableForAnAttack() > 0;
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
                .Except(new[] { attackingTerritory, defendingTerritory })
                .Union(new[] { battleResult.UpdatedAttackingTerritory, battleResult.UpdatedDefendingTerritory })
                .ToList();

            var defendingArmyAvailability = battleResult.IsDefenderDefeated() ? DefendingArmyAvailability.IsEliminated
                : DefendingArmyAvailability.Exists;

            return new AttackOutcome(updatedTerritories, defendingArmyAvailability);
        }
    }

    public enum DefendingArmyAvailability
    {
        Exists,
        IsEliminated
    }

    public class AttackOutcome
    {
        public IReadOnlyList<ITerritory> Territories { get; }
        public DefendingArmyAvailability DefendingArmyAvailability { get; }

        public AttackOutcome(IReadOnlyList<ITerritory> territories, DefendingArmyAvailability defendingArmyAvailability)
        {
            Territories = territories;
            DefendingArmyAvailability = defendingArmyAvailability;
        }
    }
}