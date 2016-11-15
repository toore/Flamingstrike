using System.Collections.Generic;
using RISK.GameEngine.Attacking;

namespace Tests.RISK.GameEngine.Builders
{
    public class DiceBuilder
    {
        public Dice Build()
        {
            return new Dice(new List<int>(), new List<int>());
        }
    }
}