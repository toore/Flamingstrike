using System.Collections.Generic;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public interface IDiceValueCalculator
    {
        int CalculateAttackerCasualties(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender);
        int CalculateDefenderCasualties(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender);
    }
}