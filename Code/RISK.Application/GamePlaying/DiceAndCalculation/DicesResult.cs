using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class DicesResult : IDicesResult
    {
        public DicesResult(ICasualtyEvaluator diceEvaluator, IEnumerable<DiceValue> attackersDices, IEnumerable<DiceValue> defendersDices)
        {
            var attackerClosure = attackersDices.ToList();
            var defenderClosure = defendersDices.ToList();

            AttackersDices = attackerClosure;
            DefendersDices = defenderClosure;

            AttackerCasualties = diceEvaluator.GetAttackerCasualties(attackerClosure, defenderClosure);
            DefenderCasualties = diceEvaluator.GetDefenderCasualties(attackerClosure, defenderClosure);
        }

        public IEnumerable<DiceValue> AttackersDices { get; private set; }
        public IEnumerable<DiceValue> DefendersDices { get; private set; }

        public int AttackerCasualties { get; private set; }
        public int DefenderCasualties { get; private set; }
    }
}