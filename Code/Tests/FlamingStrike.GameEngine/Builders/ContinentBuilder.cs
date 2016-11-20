using FlamingStrike.GameEngine;

namespace Tests.FlamingStrike.GameEngine.Builders
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