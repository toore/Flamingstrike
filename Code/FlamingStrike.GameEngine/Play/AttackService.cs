using System;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackService
    {
        bool CanAttack(ITerritory attacking, ITerritory defending);
        DefendingArmyStatus Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
    }

    public class AttackService : IAttackService
    {
        private readonly IWorldMap _worldMap;
        private readonly IDice _dice;
        private readonly IArmiesLostCalculator _armiesLostCalculator;

        public AttackService(IWorldMap worldMap, IDice dice, IArmiesLostCalculator armiesLostCalculator)
        {
            _worldMap = worldMap;
            _dice = dice;
            _armiesLostCalculator = armiesLostCalculator;
        }

        public bool CanAttack(ITerritory attacking, ITerritory defending)
        {
            var canAttack =
                HasBorder(attacking, defending)
                &&
                IsAttackerAndDefenderDifferentPlayers(attacking, defending)
                &&
                HasEnoughArmiesToPerformAttack(attacking);

            return canAttack;
        }

        private bool HasBorder(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return _worldMap.HasBorder(attackingTerritory.Region, defendingTerritory.Region);
        }

        private static bool IsAttackerAndDefenderDifferentPlayers(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Name != defendingTerritory.Name;
        }

        private static bool HasEnoughArmiesToPerformAttack(ITerritory attackingTerritory)
        {
            return attackingTerritory.GetNumberOfArmiesThatCanAttack() > 0;
        }

        public DefendingArmyStatus Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            if (!CanAttack(attackingTerritory, defendingTerritory))
            {
                throw new InvalidOperationException("Can't attack");
            }

            var numberOfArmiesUsedInAnAttack = attackingTerritory.GetNumberOfArmiesUsedInAnAttack();

            var dices = _dice.Roll(numberOfArmiesUsedInAnAttack, defendingTerritory.GetNumberOfDefendingArmies());

            var attackerLosses = _armiesLostCalculator.CalculateAttackerLosses(dices.AttackValues, dices.DefenceValues);
            attackingTerritory.RemoveArmies(attackerLosses);

            var attackingArmiesAfterCasualties = numberOfArmiesUsedInAnAttack - attackerLosses;
            var defenderLosses = _armiesLostCalculator.CalculateDefenderLosses(dices.AttackValues, dices.DefenceValues);

            if (defendingTerritory.GetNumberOfDefendingArmies() - defenderLosses == 0)
            {
                defendingTerritory.Occupy(attackingTerritory.Name, attackingArmiesAfterCasualties);
                attackingTerritory.RemoveArmies(attackingArmiesAfterCasualties);
                return DefendingArmyStatus.IsEliminated;
            }

            defendingTerritory.RemoveArmies(defenderLosses);
            return DefendingArmyStatus.IsAlive;
        }
    }

    public enum DefendingArmyStatus
    {
        IsAlive,
        IsEliminated
    }
}