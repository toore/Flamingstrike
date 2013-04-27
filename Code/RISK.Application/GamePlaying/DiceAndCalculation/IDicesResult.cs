using System.Collections.Generic;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public interface IDicesResult
    {
        IEnumerable<DiceValue> AttackersDices { get; }
        IEnumerable<DiceValue> DefendersDices { get; }
        int AttackerCasualties { get; }
        int DefenderCasualties { get; }
    }
}