using System;
using RISK.Application.GamePlaying.DiceAndCalculation;

namespace RISK.Application.GamePlaying
{
    public interface IBattleCalculator
    {
        void Attack(ITerritory attacker, ITerritory defender);
    }

    public class BattleCalculator : IBattleCalculator
    {
        private readonly IDices _dices;

        public BattleCalculator(IDices dices)
        {
            _dices = dices;
        }

        public void Attack(ITerritory attacker, ITerritory defender)
        {
            var attackingArmies = Math.Min(attacker.GetArmiesAvailableForAttack(), 3);
            var defendingArmies = Math.Min(defender.Armies, 2);

            var diceResult = _dices.Roll(attackingArmies, defendingArmies);

            attacker.Armies -= diceResult.AttackerCasualties;
            defender.Armies -= diceResult.DefenderCasualties;

            if (IsDefenderDefeated(defender))
            {
                OccupyTerritory(attacker, defender);
            }
        }

        private static void OccupyTerritory(ITerritory attacker, ITerritory defender)
        {
            const int armiesLeftBehind = 1;

            defender.Occupant = attacker.Occupant;
            defender.Armies = attacker.Armies - armiesLeftBehind;
            attacker.Armies = armiesLeftBehind;
        }

        private static bool IsDefenderDefeated(ITerritory defender)
        {
            return defender.Armies == 0;
        }
    }
}