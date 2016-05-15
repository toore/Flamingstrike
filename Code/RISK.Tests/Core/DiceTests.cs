using FluentAssertions;
using NSubstitute;
using RISK.Core;
using Toore.Shuffling;
using Xunit;

namespace RISK.Tests.Core
{
    public class DiceTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(6)]
        public void Six_face_dice_is_rolled(int value)
        {
            var randomWrapper = Substitute.For<IRandomWrapper>();
            var sut = new Dice(randomWrapper);
            randomWrapper.Next(1, 7).Returns(value);

            var actual = sut.Roll();

            actual.Should().Be(value);
        }
    }
}