using System.Collections.Generic;

namespace RISK.Domain.GamePlaying
{
    public interface IRandomSorter
    {
        IEnumerable<T> RandomSort<T>(IEnumerable<T> collection);
    }
}