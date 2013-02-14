using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Extensions;

namespace RISK.Tests.Extensions
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
        public void ToObservableCollection_returns_observable_collection()
        {
            IEnumerable<object> collection = new object[] { "first element", "second element" };

            var observableCollection = collection.ToObservableCollection();

            observableCollection.Should().BeOfType<ObservableCollection<object>>();
            observableCollection.Count.Should().Be(2);
        }
    }
}