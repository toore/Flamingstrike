using System;
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

        public static List<T> ReplaceFirstMatchingElement<T>(this List<T> collection, T element, Func<T, bool> predicate)
        {
            if (collection.Count(predicate) != 1)
            {
                throw new NotSupportedException("Could not find a single element matching predicate.");
            }

            var newCollection = collection.ToList();

            var updateIndex = collection.FindIndex(x => predicate(x));
            newCollection.RemoveAt(updateIndex);
            newCollection.Insert(updateIndex, element);

            return newCollection;
        }
    }
}