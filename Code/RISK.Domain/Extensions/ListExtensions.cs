using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.Extensions
{
    public static class ListExtensions
    {
        public static T GetNextOrFirst<T>(this IList<T> collection, T element)
        {
            var elementIndex = collection.IndexOf(element);
            var lastIndex = collection.Count - 1;
            if (elementIndex == lastIndex)
            {
                return collection.First();
            }

            var nextIndex = elementIndex + 1;
            return collection.ElementAtOrDefault(nextIndex);
        } 
    }
}