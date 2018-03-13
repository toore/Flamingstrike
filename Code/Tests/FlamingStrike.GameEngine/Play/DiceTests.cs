using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Attacking
{
    public class DiceTests
    {
        private readonly Dice _sut;
        private readonly IDie _die;

        public DiceTests()
        {
            _die = Substitute.For<IDie>();

            _sut = new Dice(_die);
        }

        [Fact]
        public void Rolls_one_attacking_dice_and_one_defending_dice()
        {
            _die.Roll().Returns(1, 2);

            var dices = _sut.Roll(1, 1);

            dices.Should().BeEquivalentTo(new DiceResult(new[] { 1 }, new[] { 2 }));
        }

        [Fact]
        public void Rolls_three_attacking_dices_and_two_defending_dices()
        {
            _die.Roll().Returns(1, 2, 3, 4, 5);

            var dices = _sut.Roll(3, 2);

            dices.Should().BeEquivalentTo(new DiceResult(new[] { 1, 2, 3 }, new[] { 4, 5 }));
        }
    }
}