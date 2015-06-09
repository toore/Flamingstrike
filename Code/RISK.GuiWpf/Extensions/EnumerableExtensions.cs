using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GuiWpf.Extensions
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
    }
}