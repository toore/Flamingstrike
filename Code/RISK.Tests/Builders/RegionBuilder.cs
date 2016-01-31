using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class RegionBuilder
    {
        private readonly Continent _continent = Continent.Europe;

        public Region Build()
        {
            var region = new Region(_continent);

            return region;
        }
    }
}