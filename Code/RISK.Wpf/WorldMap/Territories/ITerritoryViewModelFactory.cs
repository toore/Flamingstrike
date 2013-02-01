using System.Collections.Generic;

namespace RISK.WorldMap.Territories
{
    public interface ITerritoryViewModelFactory
    {
        IEnumerable<TerritoryViewModelBase> Create();
    }
}