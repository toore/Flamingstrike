using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Extensions
{
    public static class EnumerableExtensions
    {
        public static Sequence<T> ToSequence<T>(this IEnumerable<T> items)
        {
            return new Sequence<T>(items.ToArray());
        }
    }
}