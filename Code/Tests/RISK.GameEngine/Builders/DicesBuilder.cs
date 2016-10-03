using System.Collections.Generic;
using RISK.GameEngine.Attacking;

namespace Tests.RISK.GameEngine.Builders
{
    public class DicesBuilder
    {
        public Dices Build()
        {
            return new Dices(new List<int>(), new List<int>());
        }
    }
}