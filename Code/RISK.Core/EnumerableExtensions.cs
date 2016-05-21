using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
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
    }
}