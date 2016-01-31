using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class RegionBuilder
    {
        private readonly Continent _continent = Continent.Europe;

        public Region Build()
        {
            var territory = new Region(_continent);

            return territory;
        }
    }
}