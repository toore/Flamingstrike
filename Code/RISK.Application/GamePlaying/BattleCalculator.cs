using RISK.Application.Entities;
using RISK.Application.GamePlaying.DiceAndCalculation;

namespace RISK.Application.GamePlaying
{
    public class BattleCalculator : IBattleCalculator
    {
        private readonly IDices _dices;

        public BattleCalculator(IDices dices)
        {
            _dices = dices;
        }

        public void Attack(ITerritory attacker, ITerritory defender)
        {
            var diceResult = _dices.Roll(attacker.GetArmiesAvailableForAttack(), defender.Armies);

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