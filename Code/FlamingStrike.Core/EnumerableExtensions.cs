using System.Collections.Generic;

namespace FlamingStrike.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> items, T oldItem, T newItem)
        {
            foreach (var item in items)
            {
                if (Equals(oldItem, item))
                {
                    yield return newItem;
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}