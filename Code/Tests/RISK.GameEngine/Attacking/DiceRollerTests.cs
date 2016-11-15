using FluentAssertions;
using NSubstitute;
using RISK.GameEngine.Attacking;
using Xunit;

namespace Tests.RISK.GameEngine.Attacking
{
    public class DiceRollerTests
    {
        private readonly DiceRoller _sut;
        private readonly IDie _die;

        public DiceRollerTests()
        {
            _die = Substitute.For<IDie>();

            _sut = new DiceRoller(_die);
        }

        [Fact]
        public void Rolls_one_attacking_dice_and_one_defending_dice()
        {
            _die.Roll().Returns(1, 2);

            var dices = _sut.Roll(1, 1);

            dices.ShouldBeEquivalentTo(new Dice(new[] { 1 }, new[] { 2 }));
        }

        [Fact]
        public void Rolls_three_attacking_dices_and_two_defending_dices()
        {
            _die.Roll().Returns(1, 2, 3, 4, 5);

            var dices = _sut.Roll(3, 2);

            dices.ShouldBeEquivalentTo(new Dice(new[] { 1, 2, 3 }, new[] { 4, 5 }));
        }
    }
}