using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlamingStrike.UI.WPF.Extensions
{
    public static class EnumerableExtensions
    {
        public static void Add<T>(this IList<T> source, IEnumerable<T> range)
        {
            foreach (var r in range)
            {
                source.Add(r);
            }
        }
    }
}