using System.Collections.Generic;
using System.Linq;

namespace RISK.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static IList<T> AsList<T>(this T obj)
        {
            return new List<T> { obj };
        }

        public static bool IsIn<T>(this T obj, IEnumerable<T> list)
        {
            return list.Contains(obj);
        }

        public static bool IsIn<T>(this T obj, params T[] list)
        {
            return list.Contains(obj);
        }

        public static bool IsNotIn<T>(this T obj, IEnumerable<T> list)
        {
            return !list.Contains(obj);
        }

        public static bool IsNotIn<T>(this T obj, params T[] list)
        {
            return !list.Contains(obj);
        }
    }
}