using RISK.Core;
using RISK.GameEngine;

namespace RISK.Tests.Builders
{
    public class ContinentBuilder
    {
        private int _bonus = 0;

        public Continent Build()
        {
            return new Continent(_bonus);
        }
    }
}