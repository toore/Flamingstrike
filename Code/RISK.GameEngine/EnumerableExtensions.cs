using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine
{
    public static class EnumerableExtensions
    {
        public static CircularBuffer<T> ToCircularBuffer<T>(this IEnumerable<T> items)
        {
            return new CircularBuffer<T>(items.ToArray());
        }
    }
}