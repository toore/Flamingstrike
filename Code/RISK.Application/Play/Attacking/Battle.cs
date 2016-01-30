using System;

namespace RISK.Application.Play.Attacking
{
    public interface IBattle
    {
        IBattleResult Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
    }

    public class Battle : IBattle
    {
        private readonly IDicesRoller _dicesRoller;
        private readonly IBattleOutcomeCalculator _battleOutcomeCalculator;

        public Battle(IDicesRoller dicesRoller, IBattleOutcomeCalculator battleOutcomeCalculator)
        {
            _dicesRoller = dicesRoller;
            _battleOutcomeCalculator = battleOutcomeCalculator;
        }

        public IBattleResult Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var attackingArmies = Math.Min(attackingTerritory.GetNumberOfArmiesAvailableForAttack(), 3);
            var defendingArmies = Math.Min(defendingTerritory.GetNumberOfArmiesUsedForDefence(), 2);

            var dices = _dicesRoller.Roll(attackingArmies, defendingArmies);
            var battleOutcome = _battleOutcomeCalculator.Battle(dices.AttackValues, dices.DefenceValues);

            var isDefenderDefeated = IsDefenderDefeated(defendingTerritory, battleOutcome);
            if (isDefenderDefeated)
            {
                return AttackerOccupiesNewTerritory(attackingArmies, attackingTerritory, defendingTerritory);
            }

            return UpdateArmyCount(battleOutcome, attackingTerritory, defendingTerritory);
        }

        private static IBattleResult AttackerOccupiesNewTerritory(int attackingArmies, ITerritory attackingTerritory, ITerritory territoryToBeOccupied)
        {
            var attackingArmiesLeft = attackingTerritory.Armies - attackingArmies;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Player, attackingArmiesLeft);

            var occupyingPlayer = attackingTerritory.Player;
            var occupiedTerritory = new Territory(territoryToBeOccupied.Region, occupyingPlayer, attackingArmies);

            return new BattleResult(updatedAttackingTerritory, occupiedTerritory);
        }

        private static IBattleResult UpdateArmyCount(BattleOutcome battleOutcome, ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            var updatedAttackingArmies = attackingTerritory.Armies - battleOutcome.AttackerLosses;
            var updatedAttackingTerritory = new Territory(attackingTerritory.Region, attackingTerritory.Player, updatedAttackingArmies);

            var updatedAttackedArmies = defendingTerritory.Armies - battleOutcome.DefenderLosses;
            var updatedAttackedTerritory = new Territory(defendingTerritory.Region, defendingTerritory.Player, updatedAttackedArmies);

            return new BattleResult(updatedAttackingTerritory, updatedAttackedTerritory);
        }

        private static bool IsDefenderDefeated(ITerritory defendingTerritory, BattleOutcome battleOutcome)
        {
            var defendingArmiesLeftAfterAttack = defendingTerritory.Armies - battleOutcome.DefenderLosses;
            var isAttackedTerritoryDefeated = defendingArmiesLeftAfterAttack == 0;

            return isAttackedTerritoryDefeated;
        }
    }
}