using System;

namespace RISK.Core
{
    public interface IBattle
    {
        IBattleOutcome Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
    }

    public class Battle : IBattle
    {
        private readonly IDicesRoller _dicesRoller;
        private readonly IArmiesLostCalculator _armiesLostCalculator;

        public Battle(IDicesRoller dicesRoller, IArmiesLostCalculator armiesLostCalculator)
        {
            _dicesRoller = dicesRoller;
            _armiesLostCalculator = armiesLostCalculator;
        }

        public IBattleOutcome Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var attackingArmies = Math.Min(attackingTerritory.GetNumberOfArmiesAvailableForAttack(), 3);
            var defendingArmies = Math.Min(defendingTerritory.GetNumberOfArmiesUsedForDefence(), 2);

            var dices = _dicesRoller.Roll(attackingArmies, defendingArmies);
            var battleOutcome = _armiesLostCalculator.Calculate(dices.AttackValues, dices.DefenceValues);

            var isDefenderDefeated = IsDefenderDefeated(defendingTerritory, battleOutcome);
            if (isDefenderDefeated)
            {
                return AttackerOccupiesNewTerritory(attackingArmies, attackingTerritory, defendingTerritory);
            }

            return UpdateArmies(battleOutcome, attackingTerritory, defendingTerritory);
        }

        private static IBattleOutcome AttackerOccupiesNewTerritory(int attackingArmies, ITerritory attackingTerritory, ITerritory territoryToBeOccupied)
        {
            var attackingArmiesLeft = attackingTerritory.Armies - attackingArmies;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Player, attackingArmiesLeft);

            var occupyingPlayer = attackingTerritory.Player;
            var occupiedTerritory = new Territory(territoryToBeOccupied.Region, occupyingPlayer, attackingArmies);

            return new BattleOutcome(updatedAttackingTerritory, occupiedTerritory);
        }

        private static IBattleOutcome UpdateArmies(ArmiesLost armiesLost, ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var updatedAttackingArmies = attackingTerritory.Armies - armiesLost.AttackingArmiesLost;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Player, updatedAttackingArmies);

            var updatedAttackedArmies = defendingTerritory.Armies - armiesLost.DefendingArmiesLost;
            var updatedAttackedTerritory = new Territory(defendingTerritory.Region, defendingTerritory.Player, updatedAttackedArmies);

            return new BattleOutcome(updatedAttackingTerritory, updatedAttackedTerritory);
        }

        private static bool IsDefenderDefeated(ITerritory defendingTerritory, ArmiesLost armiesLost)
        {
            var defendingArmiesLeftAfterAttack = defendingTerritory.Armies - armiesLost.DefendingArmiesLost;
            var isAttackedTerritoryDefeated = defendingArmiesLeftAfterAttack == 0;

            return isAttackedTerritoryDefeated;
        }
    }
}