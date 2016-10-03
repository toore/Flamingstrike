using FluentAssertions;
using NSubstitute;
using RISK.GameEngine.Attacking;
using RISK.GameEngine.Shuffling;
using Xunit;

namespace Tests.RISK.GameEngine.Attacking
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