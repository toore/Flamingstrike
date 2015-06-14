using System.Collections.Generic;

namespace RISK.Application.Shuffling
{
    public interface IShuffler
    {
        IEnumerable<T> Shuffle<T>(IEnumerable<T> elements);
    }
}