using System.Collections.Generic;
using RISK.GameEngine.Attacking;

namespace Tests.RISK.GameEngine.Builders
{
    public class DiceResultBuilder
    {
        public DiceResult Build()
        {
            return new DiceResult(new List<int>(), new List<int>());
        }
    }
}