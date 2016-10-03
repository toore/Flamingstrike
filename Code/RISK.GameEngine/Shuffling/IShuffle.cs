using System.Collections.Generic;

namespace Toore.Shuffling
{
    public interface IShuffle
    {
        IList<T> Shuffle<T>(IEnumerable<T> elements);
    }
}