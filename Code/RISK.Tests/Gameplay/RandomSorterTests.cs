using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using Rhino.Mocks;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class RandomSorterTests
    {
        private IRandomWrapper _randomWrapper;
        private RandomSorter _randomSorter;

        [SetUp]
        public void SetUp()
        {
            _randomWrapper = MockRepository.GenerateStub<IRandomWrapper>();

            _randomSorter = new RandomSorter(_randomWrapper);
        }

        [Test]
        public void OrderByRandomOrder_randomizes_three_elements()
        {
            IEnumerable<object> sequence = new object[] { "first element", "second element", "third element" };
            _randomWrapper.Stub(x => x.Next(3)).Return(2);
            _randomWrapper.Stub(x => x.Next(2)).Return(0);
            _randomWrapper.Stub(x => x.Next(1)).Return(0);

            var orderedSequence = _randomSorter.RandomSort(sequence);

            orderedSequence.First().Should().Be("third element");
            orderedSequence.Second().Should().Be("first element");
            orderedSequence.Third().Should().Be("second element");
        }
    }
}