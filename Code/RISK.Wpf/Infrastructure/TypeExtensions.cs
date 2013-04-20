using System;
using System.Linq;

namespace GuiWpf.Infrastructure
{
    public static class TypeExtensions
    {
        public static bool Implements<T>(this Type type)
        {
            return type.GetInterfaces().ToList().Contains(typeof(T));
        }
    }
}