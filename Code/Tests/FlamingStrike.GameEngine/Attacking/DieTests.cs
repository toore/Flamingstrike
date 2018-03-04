using FlamingStrike.GameEngine.Attacking;
using FluentAssertions;
using NSubstitute;
using Toore.Shuffling;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Attacking
{
    public class DieTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(6)]
        public void Six_face_dice_is_rolled(int value)
        {
            var randomWrapper = Substitute.For<IRandomWrapper>();
            var sut = new Die(randomWrapper);
            randomWrapper.Next(1, 7).Returns(value);

            var actual = sut.Roll();

            actual.Should().Be(value);
        }
    }
}