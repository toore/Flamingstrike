﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FluentAssertions;
using GuiWpf.Extensions;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ToObservableCollection_returns_observable_collection()
        {
            IEnumerable<object> collection = new object[] { "first element", "second element" };

            var observableCollection = collection.ToObservableCollection();

            observableCollection.Should().BeOfType<ObservableCollection<object>>();
            observableCollection.Count.Should().Be(2);
        }

        [Fact]
        public void Add_adds_range()
        {
            IList<string> collection = new List<string> { "first", "second" };
            IEnumerable<string> range = new[] { "third", "fourth" };

            collection.Add(range);

            collection.Should().ContainInOrder("first", "second", "third", "fourth");
        }

        [Fact]
        public void IsEmpty()
        {
            new object[0].IsEmpty().Should().BeTrue();
            new object[1].IsEmpty().Should().BeFalse();
            new object[] { 1, 2, 3 }.IsEmpty().Should().BeFalse();
        }
    }
}