using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class TerritoryGeographyBuilder
    {
        private readonly Continent _continent = Continent.Europe;

        public TerritoryGeography Build()
        {
            var territory = new TerritoryGeography(_continent);

            return territory;
        }
    }
}