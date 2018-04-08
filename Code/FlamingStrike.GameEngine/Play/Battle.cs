using System;

namespace FlamingStrike.GameEngine.Play
{
    public interface IBattle
    {
        IAttackResult Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
    }

    public class Battle : IBattle
    {
        private readonly IDice _dice;
        private readonly IArmiesLostCalculator _armiesLostCalculator;
        private const int MaxNumberOfAttackingArmies = 3;
        private const int MaxNumberOfDefendingArmies = 2;

        public Battle(IDice dice, IArmiesLostCalculator armiesLostCalculator)
        {
            _dice = dice;
            _armiesLostCalculator = armiesLostCalculator;
        }

        public IAttackResult Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var attackingArmies = Math.Min(attackingTerritory.GetNumberOfArmiesThatAreAvailableForAnAttack(), MaxNumberOfAttackingArmies);
            var defendingArmies = Math.Min(defendingTerritory.GetNumberOfArmiesUsedAsDefence(), MaxNumberOfDefendingArmies);

            var dices = _dice.Roll(attackingArmies, defendingArmies);
            var battleOutcome = _armiesLostCalculator.Calculate(dices.AttackValues, dices.DefenceValues);

            var isDefenderDefeated = IsDefenderDefeated(defendingTerritory, battleOutcome);
            if (isDefenderDefeated)
            {
                return AttackerOccupiesNewTerritory(attackingArmies, attackingTerritory, defendingTerritory);
            }

            return UpdateArmies(battleOutcome, attackingTerritory, defendingTerritory);
        }

        private static IAttackResult AttackerOccupiesNewTerritory(int attackingArmies, ITerritory attackingTerritory, ITerritory territoryToBeOccupied)
        {
            var attackingArmiesLeft = attackingTerritory.Armies - attackingArmies;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Name, attackingArmiesLeft);

            var occupyingPlayer = attackingTerritory.Name;
            var occupiedTerritory = new Territory(territoryToBeOccupied.Region, occupyingPlayer, attackingArmies);

            return new AttackResult(updatedAttackingTerritory, occupiedTerritory);
        }

        private static IAttackResult UpdateArmies(ArmiesLost armiesLost, ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var updatedAttackingArmies = attackingTerritory.Armies - armiesLost.AttackingArmiesLost;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Name, updatedAttackingArmies);

            var updatedAttackedArmies = defendingTerritory.Armies - armiesLost.DefendingArmiesLost;
            var updatedAttackedTerritory = new Territory(defendingTerritory.Region, defendingTerritory.Name, updatedAttackedArmies);

            return new AttackResult(updatedAttackingTerritory, updatedAttackedTerritory);
        }

        private static bool IsDefenderDefeated(ITerritory defendingTerritory, ArmiesLost armiesLost)
        {
            var defendingArmiesLeftAfterAttack = defendingTerritory.Armies - armiesLost.DefendingArmiesLost;
            var isAttackedTerritoryDefeated = defendingArmiesLeftAfterAttack == 0;

            return isAttackedTerritoryDefeated;
        }
    }
}