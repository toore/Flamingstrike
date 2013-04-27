using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public class DicesResult : IDicesResult
    {
        public DicesResult(IDiceValueCalculator diceEvaluator, IEnumerable<DiceValue> attackersDices, IEnumerable<DiceValue> defendersDices)
        {
            var attackerClosure = attackersDices.ToList();
            var defenderClosure = defendersDices.ToList();

            AttackersDices = attackerClosure;
            DefendersDices = defenderClosure;

            AttackerCasualties = diceEvaluator.CalculateAttackerCasualties(attackerClosure, defenderClosure);
            DefenderCasualties = diceEvaluator.CalculateDefenderCasualties(attackerClosure, defenderClosure);
        }

        public IEnumerable<DiceValue> AttackersDices { get; private set; }
        public IEnumerable<DiceValue> DefendersDices { get; private set; }

        public int AttackerCasualties { get; private set; }
        public int DefenderCasualties { get; private set; }
    }
}