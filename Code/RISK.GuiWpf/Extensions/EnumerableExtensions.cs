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
    }
}