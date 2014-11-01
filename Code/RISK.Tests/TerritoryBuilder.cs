using RISK.Domain.Entities;

namespace RISK.Tests
{
    public class TerritoryBuilder
    {
        private ILocation _location;
        private IPlayer _occupant;

        public Territory Build()
        {
            return new Territory(_location)
            {
                Occupant = _occupant
            };
        }

        public TerritoryBuilder Location(ILocation location)
        {
            _location = location;
            return this;
        }

        public TerritoryBuilder Occupant(IPlayer occupant)
        {
            _occupant = occupant;
            return this;
        }
    }
}