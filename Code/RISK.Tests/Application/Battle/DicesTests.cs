using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Application.GamePlaying.DiceAndCalculation;
using Xunit;

namespace RISK.Tests.Application.Battle
{
    public class DicesTests
    {
        private readonly Dices _dices;
        private readonly IDice _dice;
        private readonly ICasualtiesCalculator _casualtiesCalculator;

        public DicesTests()
        {
            _casualtiesCalculator = Substitute.For<ICasualtiesCalculator>();
            _dice = Substitute.For<IDice>();

            _dices = new Dices(_casualtiesCalculator, _dice);
        }

        [Fact]
        public void Attacking_1_defending_1_has_1_attacking_dice_and_one_defending_dice()
        {
            var expectedCasualties = new Casualties { AttackerCasualties = 11, DefenderCasualties = 22 };
            _casualtiesCalculator.CalculateCasualties(
                Arg.Is<IEnumerable<int>>(x => x.SequenceEqual(new[] { 1 })),
                Arg.Is<IEnumerable<int>>(x => x.SequenceEqual(new[] { 2 })))
                .Returns(expectedCasualties);
            _dice.Roll().Returns(1, 2);

            var dicesResult = _dices.Roll(1, 1);

            dicesResult.AttackerCasualties.Should().Be(11);
            dicesResult.DefenderCasualties.Should().Be(22);
        }
    }
}