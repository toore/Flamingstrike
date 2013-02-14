using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RISK.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static T Second<T>(this IEnumerable<T> sequence)
        {
            return sequence.ElementAt(1);
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        {
            return new ObservableCollection<T>(collection);
        }
    }
}