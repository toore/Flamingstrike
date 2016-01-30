using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class TerritoryGeographyBuilder
    {
        private readonly Continent _continent = Continent.Europe;

        public Region Build()
        {
            var territory = new Region(_continent);

            return territory;
        }
    }
}