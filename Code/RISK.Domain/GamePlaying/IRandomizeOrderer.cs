using System.Collections.Generic;

namespace RISK.Domain.GamePlaying
{
    public interface IRandomizeOrderer
    {
        IEnumerable<T> OrderByRandomOrder<T>(IEnumerable<T> collection);
    }
}