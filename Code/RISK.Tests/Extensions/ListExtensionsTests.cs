using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Extensions;

namespace RISK.Tests.Extensions
{
    [TestFixture]
    public class ListExtensionsTests
    {
        [Test]
        public void GetNextOrFirst_returns_first_element_when_null_element()
        {
            IList<object> sequence = new object[] { "first element", "second element", "third element", "fourth element" };

            sequence.GetNextOrFirst(null).Should().Be("first element");
        }

        [Test]
        public void GetNextOrFirst_returns_first_element_when_last_given()
        {
            IList<object> sequence = new object[] { "first element", "second element", "third element", "fourth element" };

            sequence.GetNextOrFirst("fourth element").Should().Be("first element");
        }

        [Test]
        public void GetNextOrFirst_returns_second_element()
        {
            IList<object> sequence = new object[] { "first element", "second element", "third element", "fourth element" };

            sequence.GetNextOrFirst("first element").Should().Be("second element");
        }
    }
}