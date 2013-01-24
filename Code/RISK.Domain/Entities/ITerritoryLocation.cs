using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface ITerritoryLocation
    {
        string TranslationKey { get; }
        Continent Continent { get; }
        IEnumerable<ITerritoryLocation> ConnectedTerritories { get; }
    }
}