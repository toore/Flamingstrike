using System.Collections.Generic;
using RISK.Application.World;

namespace RISK.Tests.Builders
{
    public class TerritoryBuilder
    {
        private string _name = "";
        private Continent _continent = Continent.Europe;
        private readonly List<ITerritory> _borders = new List<ITerritory>();

        public Territory Build()
        {
            var territory = new Territory(_name, _continent);
            territory.AddBorderToTerritories(_borders.ToArray());

            return territory;
        }

        public TerritoryBuilder WithBorder(ITerritory location)
        {
            _borders.Add(location);
            return this;
        }
    }
}