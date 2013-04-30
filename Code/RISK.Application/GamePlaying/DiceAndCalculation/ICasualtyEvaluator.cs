using System.Collections.Generic;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public interface ICasualtyEvaluator
    {
        int GetAttackerCasualties(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender);
        int GetDefenderCasualties(IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender);
    }
}