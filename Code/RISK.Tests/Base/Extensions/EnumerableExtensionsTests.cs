using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RISK.Base.Extensions;

namespace RISK.Tests.Base.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void Second_returns_the_second_element_of_an_enumerable()
        {
            IEnumerable<object> sequence = new object[] { "first element", "second element", "third element", "fourth element" };

            sequence.Second().Should().Be("second element");
        }

        [Test]
        public void Third_returns_the_third_element_of_an_enumerable()
        {
            IEnumerable<object> sequence = new object[] { "first element", "second element", "third element", "fourth element" };

            sequence.Third().Should().Be("third element");
        }
    }
}