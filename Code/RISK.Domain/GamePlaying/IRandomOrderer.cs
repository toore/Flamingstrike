using System.Collections.Generic;

namespace RISK.Domain.GamePlaying
{
    public interface IRandomOrderer
    {
        IEnumerable<T> OrderByRandomOrder<T>(IEnumerable<T> collection);
    }
}