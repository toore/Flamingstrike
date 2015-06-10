using System.Collections.Generic;
using System.Linq;

namespace RISK.Application.Extensions
{
    public static class ListExtensions
    {
        public static T GetNextOrFirst<T>(this IList<T> collection, T element)
        {
            var nextIndex = collection.IndexOf(element) + 1;

            if (nextIndex >= collection.Count)
            {
                return collection.First();
            }

            return collection.ElementAt(nextIndex);
        } 
    }
}