using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Shuffling
{
    public interface IShuffle
    {
        IList<T> Shuffle<T>(IEnumerable<T> elements);
    }
}