using FluentAssertions;
using NSubstitute;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.DiceAndCalculation;
using Xunit;

namespace RISK.Tests.Application.Battle
{
    public class DiceTests
    {
        private readonly Dice _dice;
        private readonly IRandomWrapper _randomWrapper;

        public DiceTests()
        {
            _randomWrapper = Substitute.For<IRandomWrapper>();
            _dice = new Dice(_randomWrapper);
        }

        [Fact]
        public void Roll_returns_a_six()
        {
            AssertDice(DiceValue.Six, randomValueStub: 5);
        }

        [Fact]
        public void Roll_returns_a_one()
        {
            AssertDice(DiceValue.One, randomValueStub: 0);
        }

        private void AssertDice(DiceValue expected, int randomValueStub)
        {
            _randomWrapper.Next(6).Returns(randomValueStub);

            _dice.Roll().Should().Be(expected);
        }
    }
}