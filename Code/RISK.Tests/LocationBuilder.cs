using RISK.Domain;
using RISK.Domain.Entities;

namespace RISK.Tests
{
    public class LocationBuilder 
    {
        private string _name = "";
        private Continent _continent = Continent.Europe;

        public Location Build()
        {
            return new Location(_name, _continent);
        }
    }
}