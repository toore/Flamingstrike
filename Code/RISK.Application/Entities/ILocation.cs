using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface ILocation
    {
        string TranslationKey { get; }
        Continent Continent { get; }
        IEnumerable<ILocation> Connections { get; }
    }
}