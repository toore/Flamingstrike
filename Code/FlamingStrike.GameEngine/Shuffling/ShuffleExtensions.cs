using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Shuffling
{
    public static class ShuffleExtensions
    {
        public static IList<T> Shuffle<T>(this IEnumerable<T> elements, IShuffle shuffleAlgorithm)
        {
            return shuffleAlgorithm.Shuffle(elements);
        }
    }
}