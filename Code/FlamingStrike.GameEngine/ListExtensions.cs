using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine
{
    public static class ListExtensions
    {
        public static T GetNext<T>(this IList<T> items, T precedingItem)
        {
            if (items.Last().Equals(precedingItem))
            {
                return items.First();
            }

            return items.SkipWhile(x => !Equals(x, precedingItem))
                .Skip(1)
                .Take(1).Single();
        }
    }
}