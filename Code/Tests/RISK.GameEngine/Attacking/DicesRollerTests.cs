using FluentAssertions;
using NSubstitute;
using RISK.GameEngine.Attacking;
using Xunit;

namespace Tests.RISK.GameEngine.Attacking
{
    public class DicesRollerTests
    {
        private readonly DicesRoller _sut;
        private readonly IDice _dice;

        public DicesRollerTests()
        {
            _dice = Substitute.For<IDice>();

            _sut = new DicesRoller(_dice);
        }

        [Fact]
        public void Rolls_one_attacking_dice_and_one_defending_dice()
        {
            _dice.Roll().Returns(1, 2);

            var dices = _sut.Roll(1, 1);

            dices.ShouldBeEquivalentTo(new Dices(new[] { 1 }, new[] { 2 }));
        }

        [Fact]
        public void Rolls_three_attacking_dices_and_two_defending_dices()
        {
            _dice.Roll().Returns(1, 2, 3, 4, 5);

            var dices = _sut.Roll(3, 2);

            dices.ShouldBeEquivalentTo(new Dices(new[] { 1, 2, 3 }, new[] { 4, 5 }));
        }
    }
}