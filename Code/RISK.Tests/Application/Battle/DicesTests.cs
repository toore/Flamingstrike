using System.Linq;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Experimental;
using NUnit.Framework;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Tests.Application.Battle
{
    [TestFixture]
    public class DicesTests
    {
        private Dices _dices;
        private ICasualtyEvaluator _casualtyEvaluator;
        private IDiceRoller _diceRoller;

        [SetUp]
        public void SetUp()
        {
            _casualtyEvaluator = Substitute.For<ICasualtyEvaluator>();
            _diceRoller = Substitute.For<IDiceRoller>();

            _dices = new Dices(_casualtyEvaluator, _diceRoller);
        }

        [Test]
        public void Attacking_1_defending_1_has_1_attacking_dice_and_one_defending_dice()
        {
            _diceRoller.Roll().Returns(DiceValue.One, DiceValue.Two);

            var dicesResult = _dices.Roll(1, 1);

            dicesResult.AttackersDices.Count().Should().Be(1);
            dicesResult.DefendersDices.Count().Should().Be(1);
        }

        [Test]
        public void Does_not_roll_more_than_3_dices_when_attacking()
        {
            _dices.Roll(4, 1);

            _diceRoller.Received(4).Roll();
        }

        [Test]
        public void Does_not_roll_more_than_2_dices_when_defending()
        {
            _dices.Roll(1, 3);

            _diceRoller.Received(3).Roll();
        }
    }
}