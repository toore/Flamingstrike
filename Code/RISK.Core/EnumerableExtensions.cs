using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ReplaceItem<T>(this IEnumerable<T> items, T remove, T add)
        {
            if (!items.Contains(remove))
            {
                throw new ArgumentException($"{nameof(remove)} does not exist in {nameof(items)}", nameof(remove));
            }
            if (items.Contains(add))
            {
                throw new ArgumentException($"{nameof(add)} already exists in {nameof(items)}", nameof(add));
            }

            var enumerable = items
                .Except(new[] { remove })
                .Union(new[] { add });

            return enumerable;
        }
    }
}