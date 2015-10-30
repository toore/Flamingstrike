using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class TerritoryIdBuilder
    {
        private const string Name = "";
        private readonly Continent _continent = Continent.Europe;

        public TerritoryId Build()
        {
            var territory = new TerritoryId(Name, _continent);

            return territory;
        }
    }
}