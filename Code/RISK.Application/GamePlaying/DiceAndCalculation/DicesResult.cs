using System.Collections.Generic;

namespace RISK.Application.GamePlaying.DiceAndCalculation
{
    public class DicesResult : IDicesResult
    {
        public IEnumerable<DiceValue> AttackDices { get; set; }
        public IEnumerable<DiceValue> DefendDices { get; set; }

        public int AttackerCasualties { get; set; }
        public int DefenderCasualties { get; set; }
    }
}