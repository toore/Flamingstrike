using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine
{
    public static class EnumerableExtensions
    {
        public static CircularBuffer<T> ToCircularBuffer<T>(this IEnumerable<T> items)
        {
            return new CircularBuffer<T>(items.ToArray());
        }

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