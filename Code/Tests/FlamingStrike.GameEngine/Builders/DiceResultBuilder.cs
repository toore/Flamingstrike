using System.Collections.Generic;
using FlamingStrike.GameEngine.Attacking;

namespace Tests.FlamingStrike.GameEngine.Builders
{
    public class DiceResultBuilder
    {
        public DiceResult Build()
        {
            return new DiceResult(new List<int>(), new List<int>());
        }
    }
}