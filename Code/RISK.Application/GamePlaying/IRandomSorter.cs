using System.Collections.Generic;

namespace RISK.Domain.GamePlaying
{
    public interface IRandomSorter
    {
        IEnumerable<T> Sort<T>(IEnumerable<T> collection);
    }
}