using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using Xunit;

namespace RISK.Tests.Application.Battle
{
    public class DicesTests
    {
        private readonly Dices _dices;
        private readonly IDice _dice;

        public DicesTests()
        {
            var casualtyEvaluator = Substitute.For<ICasualtyEvaluator>();
            _dice = Substitute.For<IDice>();

            _dices = new Dices(casualtyEvaluator, _dice);
        }

        [Fact]
        public void Attacking_1_defending_1_has_1_attacking_dice_and_one_defending_dice()
        {
            _dice.Roll().Returns(DiceValue.One, DiceValue.Two);

            var dicesResult = _dices.Roll(1, 1);

            dicesResult.AttackersDices.Count().Should().Be(1);
            dicesResult.DefendersDices.Count().Should().Be(1);
        }

        [Fact]
        public void Does_not_roll_more_than_3_dices_when_attacking()
        {
            _dices.Roll(4, 1);

            _dice.Received(4).Roll();
        }

        [Fact]
        public void Does_not_roll_more_than_2_dices_when_defending()
        {
            _dices.Roll(1, 3);

            _dice.Received(3).Roll();
        }
    }
}