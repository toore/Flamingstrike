using System;
using FluentAssertions;
using NSubstitute;
using RISK.Application.Play.Battling;
using Xunit;

namespace RISK.Tests.Application
{
    public class DiceRollerTests
    {
        private readonly DiceRoller _sut;
        private readonly IDice _dice;
        private readonly IBattleCalculator _battleCalculator;

        public DiceRollerTests()
        {
            _battleCalculator = Substitute.For<IBattleCalculator>();
            _dice = Substitute.For<IDice>();

            _sut = new DiceRoller(_dice);
        }

        [Fact]
        public void Rolls_one_attacking_dice_and_one_defending_dice()
        {
            var dices = _sut.Roll(1, 1);

            dices.ShouldBeEquivalentTo(new Dices(new[] { 1 }, new[] { 2 }));
        }

        [Fact]
        public void Attacking_1_defending_1_has_1_attacking_dice_and_one_defending_dice()
        {
            throw new NotImplementedException();
            //var expectedCasualties = new Casualties { AttackerCasualties = 11, DefenderCasualties = 22 };
            //_battleCalculator.Calculate(
            //    Arg.Is<IEnumerable<int>>(x => x.SequenceEqual(new[] { 1 })),
            //    Arg.Is<IEnumerable<int>>(x => x.SequenceEqual(new[] { 2 })))
            //    .Returns(expectedCasualties);
            //_dice.Roll().Returns(1, 2);

            //var dicesResult = _sut.Roll(1, 1);

            //dicesResult.AttackerCasualties.Should().Be(11);
            //dicesResult.DefenderCasualties.Should().Be(22);
        }
    }
}