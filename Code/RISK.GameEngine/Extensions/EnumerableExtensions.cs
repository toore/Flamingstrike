using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> items, T exclude, T include)
        {
            var updatedList = items
                .Except(new[] { exclude })
                .Union(new[] { include });

            return updatedList;
        }

        public static Sequence<T> ToSequence<T>(this IEnumerable<T> items)
        {
            return new Sequence<T>(items.ToArray());
        }
    }
}