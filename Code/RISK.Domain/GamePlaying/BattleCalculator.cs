using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Domain.GamePlaying
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
            var diceResult = _dices.Roll(attacker.Armies, defender.Armies);

            attacker.Armies -= diceResult.AttackerCasualties;
            defender.Armies -= diceResult.DefenderCasualties;

            if (IsDefenderDefeated(defender))
            {
                OccupyTerritory(attacker, defender);
            }
        }

        private void OccupyTerritory(ITerritory attacker, ITerritory defender)
        {
            const int armiesLeftBehind = 1;

            defender.Owner = attacker.Owner;
            defender.Armies = attacker.Armies - armiesLeftBehind;
            attacker.Armies = armiesLeftBehind;
        }

        private bool IsDefenderDefeated(ITerritory defender)
        {
            return defender.Armies == 0;
        }
    }
}