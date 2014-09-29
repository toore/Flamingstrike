﻿using System.Collections.Generic;
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
                Arg.Is<IEnumerable<DiceValue>>(x => x.SequenceEqual(new[] { DiceValue.One })),
                Arg.Is<IEnumerable<DiceValue>>(x => x.SequenceEqual(new[] { DiceValue.Two })))
                .Returns(expectedCasualties);
            _dice.Roll().Returns(DiceValue.One, DiceValue.Two);

            var dicesResult = _dices.Roll(1, 1);

            dicesResult.AttackerCasualties.Should().Be(11);
            dicesResult.DefenderCasualties.Should().Be(22);
        }

        [Fact]
        public void Does_not_roll_more_than_3_dices_when_attacking()
        {
            _casualtiesCalculator.CalculateCasualties(null, null).ReturnsForAnyArgs(new Casualties());

            _dices.Roll(4, 1);

            _dice.Received(4).Roll();
        }

        [Fact]
        public void Does_not_roll_more_than_2_dices_when_defending()
        {
            _casualtiesCalculator.CalculateCasualties(null, null).ReturnsForAnyArgs(new Casualties());

            _dices.Roll(1, 3);

            _dice.Received(3).Roll();
        }
    }
}