using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface IAreaDefinition
    {
        string TranslationKey { get; }
        Continent Continent { get; }
        IEnumerable<IAreaDefinition> Neighbors { get; }
    }
}