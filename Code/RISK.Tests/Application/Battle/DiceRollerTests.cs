using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Tests.Application.Battle
{
    [TestFixture]
    public class DiceRollerTests
    {
        private DiceRoller _diceRoller;
        private IRandomWrapper _randomWrapper;

        [SetUp]
        public void SetUp()
        {
            _randomWrapper = Substitute.For<IRandomWrapper>();
            _diceRoller = new DiceRoller(_randomWrapper);
        }

        [Test]
        public void Roll_returns_a_six()
        {
            AssertDice(DiceValue.Six, randomValueStub: 5);
        }

        [Test]
        public void Roll_returns_a_one()
        {
            AssertDice(DiceValue.One, randomValueStub: 0);

        }

        private void AssertDice(DiceValue expected, int randomValueStub)
        {
            _randomWrapper.Next(6).Returns(randomValueStub);

            _diceRoller.Roll().Should().Be(expected);
        }
    }
}