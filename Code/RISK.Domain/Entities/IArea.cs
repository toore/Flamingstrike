using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface IArea
    {
        string TranslationKey { get; }
        Continent Continent { get; }
        IEnumerable<IArea> Neighbors { get; }
    }
}