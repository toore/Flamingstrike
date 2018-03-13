using FlamingStrike.GameEngine;

namespace Tests.FlamingStrike.GameEngine
{
    public class RegionBuilder
    {
        private readonly Continent _continent = new ContinentBuilder().Build();

        public Region Build()
        {
            var region = new Region(_continent);

            return region;
        }
    }
}