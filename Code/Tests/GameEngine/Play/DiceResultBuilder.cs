using System.Collections.Generic;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class DiceResultBuilder
    {
        public DiceResult Build()
        {
            return new DiceResult(new List<int>(), new List<int>());
        }
    }
}