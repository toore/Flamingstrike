using System.Collections.Generic;
using RISK.Application;

namespace RISK.Tests
{
    public class TerritoryBuilder
    {
        private string _name = "";
        private Continent _continent = Continent.Europe;
        private readonly List<ITerritory> _borders = new List<ITerritory>();
        private IPlayer _occupant;
        private int _armies;

        public Territory Build()
        {
            var territory = new Territory(_name, _continent);
            territory.Occupant = _occupant;
            territory.Armies = _armies;
            territory.AddBorders(_borders.ToArray());

            return territory;
        }

        public TerritoryBuilder WithBorder(ITerritory location)
        {
            _borders.Add(location);
            return this;
        }

        public TerritoryBuilder Occupant(IPlayer occupant)
        {
            _occupant = occupant;
            return this;
        }

        public TerritoryBuilder Armies(int armies)
        {
            _armies = armies;
            return this;
        }
    }
}