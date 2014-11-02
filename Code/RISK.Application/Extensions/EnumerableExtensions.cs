using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Second<T>(this IEnumerable<T> sequence)
        {
            return sequence.ElementAt(1);
        }

        public static T Third<T>(this IEnumerable<T> sequence)
        {
            return sequence.ElementAt(2);
        }
    }
}