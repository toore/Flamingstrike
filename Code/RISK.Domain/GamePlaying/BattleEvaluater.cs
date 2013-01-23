using RISK.Domain.Entities;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Domain.GamePlaying
{
    public class BattleEvaluater : IBattleEvaluater
    {
        private readonly IDices _dices;

        public BattleEvaluater(IDices dices)
        {
            _dices = dices;
        }

        public void Attack(IArea attacker, IArea defender)
        {
            var diceResult = _dices.Roll(attacker.Armies, defender.Armies);
        }
    }
}