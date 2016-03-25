using System.Collections.Generic;
using System.Linq;

namespace RISK.Tests.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsEquivalent<T>(this IEnumerable<T> first, params T[] second)
        {
            var firstList = first.ToList();
            if (firstList.Count != second.Length)
            {
                return false;
            }

            return firstList.Intersect(second)
                .Count() == firstList.Count;
        }

        public static bool IsEquivalent<T>(this IEnumerable<T> first, IReadOnlyList<T> second)
        {
            return IsEquivalent(first, second.ToArray());
        }
    }
}