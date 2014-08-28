using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class RandomSorterTests
    {
        private IRandomWrapper _randomWrapper;
        private RandomSorter _randomSorter;

        public RandomSorterTests()
        {
            _randomWrapper = Substitute.For<IRandomWrapper>();

            _randomSorter = new RandomSorter(_randomWrapper);
        }

        [Fact]
        public void OrderByRandomOrder_randomizes_three_elements()
        {
            IEnumerable<object> sequence = new object[] { "first element", "second element", "third element" };
            _randomWrapper.Next(3).Returns(2);
            _randomWrapper.Next(2).Returns(0);
            _randomWrapper.Next(1).Returns(0);

            var orderedSequence = _randomSorter.Sort(sequence)
                .ToList();

            orderedSequence.First().Should().Be("third element");
            orderedSequence.Second().Should().Be("first element");
            orderedSequence.Third().Should().Be("second element");
        }
    }
}