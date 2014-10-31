using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface ILocation
    {
        string Name { get; }
        Continent Continent { get; }
        IEnumerable<ILocation> Borders { get; }
    }
}