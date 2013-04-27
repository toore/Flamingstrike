﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using GuiWpf.Extensions;
using NUnit.Framework;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
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