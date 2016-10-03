using System.Collections.Generic;

namespace RISK.GameEngine.Shuffling
{
    public interface IShuffle
    {
        IList<T> Shuffle<T>(IEnumerable<T> elements);
    }
}