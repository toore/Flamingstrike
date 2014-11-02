using System.Collections.Generic;

namespace RISK.Application.GamePlaying
{
    public interface IRandomSorter
    {
        IEnumerable<T> Sort<T>(IEnumerable<T> collection);
    }
}