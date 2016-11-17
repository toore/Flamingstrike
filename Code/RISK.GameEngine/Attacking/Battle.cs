using System;

namespace RISK.GameEngine.Attacking
{
    public interface IBattle
    {
        IAttackResult Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
    }

    public class Battle : IBattle
    {
        private readonly IDice _dice;
        private readonly IArmiesLostCalculator _armiesLostCalculator;

        public Battle(IDice dice, IArmiesLostCalculator armiesLostCalculator)
        {
            _dice = dice;
            _armiesLostCalculator = armiesLostCalculator;
        }

        public IAttackResult Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var attackingArmies = Math.Min(attackingTerritory.GetNumberOfArmiesThatCanBeUsedInAnAttack(), 3);
            var defendingArmies = Math.Min(defendingTerritory.GetNumberOfArmiesUsedAsDefence(), 2);

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
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Player, attackingArmiesLeft);

            var occupyingPlayer = attackingTerritory.Player;
            var occupiedTerritory = new Territory(territoryToBeOccupied.Region, occupyingPlayer, attackingArmies);

            return new AttackResult(updatedAttackingTerritory, occupiedTerritory);
        }

        private static IAttackResult UpdateArmies(ArmiesLost armiesLost, ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var updatedAttackingArmies = attackingTerritory.Armies - armiesLost.AttackingArmiesLost;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Player, updatedAttackingArmies);

            var updatedAttackedArmies = defendingTerritory.Armies - armiesLost.DefendingArmiesLost;
            var updatedAttackedTerritory = new Territory(defendingTerritory.Region, defendingTerritory.Player, updatedAttackedArmies);

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