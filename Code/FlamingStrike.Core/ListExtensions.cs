using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.Core
{
    public static class ListExtensions
    {
        public static T GetNext<T>(this IList<T> items, T item)
        {
            if (items.Last().Equals(item))
            {
                return items.First();
            }

            return items.SkipWhile(x => !Equals(x, item))
                .Skip(1)
                .Take(1)
                .Single();
        }
    }
}