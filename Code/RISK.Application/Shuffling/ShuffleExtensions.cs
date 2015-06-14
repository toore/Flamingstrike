using System.Collections.Generic;
using RISK.Application.Shuffling;

namespace Toore.Shuffling
{
    public static class ShuffleExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> elements, IShuffler shufflerAlgorithm)
        {
            var shuffledSet = shufflerAlgorithm.Shuffle(elements);
            return shuffledSet;
        }
    }
}