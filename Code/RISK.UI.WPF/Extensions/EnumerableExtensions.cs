using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RISK.UI.WPF.Extensions
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }

        public static void Add<T>(this IList<T> source, IEnumerable<T> range)
        {
            foreach (var r in range)
            {
                source.Add(r);
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return !collection.Any();
        }
    }
}