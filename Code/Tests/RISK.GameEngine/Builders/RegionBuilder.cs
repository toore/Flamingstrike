using RISK.GameEngine;

namespace Tests.RISK.GameEngine.Builders
{
    public class RegionBuilder
    {
        private readonly Continent _continent = Make.Continent.Build();

        public Region Build()
        {
            var region = new Region(_continent);

            return region;
        }
    }
}