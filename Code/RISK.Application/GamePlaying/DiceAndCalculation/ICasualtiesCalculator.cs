using System.Collections.Generic;

namespace RISK.Domain.GamePlaying.DiceAndCalculation
{
    public interface ICasualtiesCalculator
    {
        Casualties CalculateCasualties(IEnumerable<DiceValue> attackDices, IEnumerable<DiceValue> defendDices);
    }

    public class Casualties
    {
        public int AttackerCasualties { get; set; }
        public int DefenderCasualties { get; set; }
    }
}