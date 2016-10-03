using System.Collections.Generic;
using RISK.Core;

namespace Tests.Builders
{
    public class DicesBuilder
    {
        public Dices Build()
        {
            return new Dices(new List<int>(), new List<int>());
        }
    }
}