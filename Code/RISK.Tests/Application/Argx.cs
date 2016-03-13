using System.Collections.Generic;
using NSubstitute;
using RISK.Tests.Extensions;

namespace RISK.Tests.Application
{
    public static class Argx
    {
        public static IEnumerable<T> IsEquivalent<T>(params T[] array)
        {
            return Arg.Is<IEnumerable<T>>(x => x.IsEquivalent(array));
        }

        public static IReadOnlyList<T> IsEquivalentReadOnly<T>(params T[] array)
        {
            return Arg.Is<IReadOnlyList<T>>(x => x.IsEquivalent(array));
        }
    }
}