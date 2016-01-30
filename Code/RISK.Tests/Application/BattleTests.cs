using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using RISK.Application.Play.Attacking;
using RISK.Tests.Builders;
using Xunit;
using Xunit.Sdk;

namespace RISK.Tests.Application
{
    public abstract class BattleTests
    {
        private readonly Battle _sut;
        private readonly IDicesRoller _dicesRoller;
        private readonly IBattleOutcomeCalculator _battleOutcomeCalculator;

        protected BattleTests()
        {
            _dicesRoller = Substitute.For<IDicesRoller>();
            _battleOutcomeCalculator = Substitute.For<IBattleOutcomeCalculator>();

            _sut = new Battle(_dicesRoller, _battleOutcomeCalculator);
        }

        public class Uses_correct_number_of_dices_with_respect_to_number_of_armies : BattleTests
        {
            public Uses_correct_number_of_dices_with_respect_to_number_of_armies()
            {
                _dicesRoller.Roll(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(Make.Dices.Build());
                _battleOutcomeCalculator.Battle(null, null).ReturnsForAnyArgs(Make.BattleOutcome.Build());
            }

            private class NumberOfAttackingDicesCasesAttribute : DataAttribute
            {
                public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
                {
                    yield return new object[] { 5, 3 };
                    yield return new object[] { 4, 3 };
                    yield return new object[] { 2, 1 };
                }
            }

            [Theory]
            [NumberOfAttackingDicesCases]
            public void Attacker_attacks_with_a_maximum_of_three_armies(int armiesInAttackingTerritory, int expectedNumberOfAttackingDices)
            {
                var attackingTerritory = Make.Territory.Armies(armiesInAttackingTerritory).Build();
                var defendingTerritory = Make.Territory.Build();

                _sut.Attack(attackingTerritory, defendingTerritory);

                _dicesRoller.Received().Roll(expectedNumberOfAttackingDices, Arg.Any<int>());
            }

            private class NumberOfDefendingDicesCasesAttribute : DataAttribute
            {
                public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
                {
                    yield return new object[] { 3, 2 };
                    yield return new object[] { 2, 2 };
                    yield return new object[] { 1, 1 };
                }
            }

            [Theory]
            [NumberOfDefendingDicesCases]
            public void Defender_defends_with_a_maximum_of_two_armies(int armiesInDefendingTerritory, int expectedNumberOfDefendingDices)
            {
                var attackingTerritory = Make.Territory.Build();
                var defendingTerritory = Make.Territory.Armies(armiesInDefendingTerritory).Build();

                _sut.Attack(attackingTerritory, defendingTerritory);

                _dicesRoller.Received().Roll(Arg.Any<int>(), expectedNumberOfDefendingDices);
            }
        }

        public class Territories_are_updated_after_battle : BattleTests
        {
            [Fact]
            public void Attacker_loses_one()
            {
                var attackingTerritory = Make.Territory.Armies(3).Build();
                var defendingTerritory = Make.Territory.Armies(1).Build();
                var attackerLosesOne = new BattleOutcome(1, 0);
                SetActualBattleOutcome(2, 1, attackerLosesOne);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.AttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.AttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.AttackingTerritory.Armies.Should().Be(2);
                battleResult.DefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.DefendingTerritory.Player.Should().Be(defendingTerritory.Player);
                battleResult.DefendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Defender_loses_one()
            {
                var attackingTerritory = Make.Territory.Armies(2).Build();
                var defendingTerritory = Make.Territory.Armies(2).Build();
                var defenderLosesOne = new BattleOutcome(0, 1);
                SetActualBattleOutcome(1, 2, defenderLosesOne);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.AttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.AttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.AttackingTerritory.Armies.Should().Be(2);
                battleResult.DefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.DefendingTerritory.Player.Should().Be(defendingTerritory.Player);
                battleResult.DefendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Its_a_draw()
            {
                var attackingTerritory = Make.Territory.Armies(4).Build();
                var defendingTerritory = Make.Territory.Armies(2).Build();
                var draw = new BattleOutcome(1, 1);
                SetActualBattleOutcome(3, 2, draw);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.AttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.AttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.AttackingTerritory.Armies.Should().Be(3);
                battleResult.DefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.DefendingTerritory.Player.Should().Be(defendingTerritory.Player);
                battleResult.DefendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Defender_is_defeated_and_occupied()
            {
                var attackingTerritory = Make.Territory.Armies(2).Build();
                var defendingTerritory = Make.Territory.Armies(1).Build();
                var defenderLosesOne = new BattleOutcome(0, 1);
                SetActualBattleOutcome(1, 1, defenderLosesOne);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.AttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.AttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.AttackingTerritory.Armies.Should().Be(1);
                battleResult.DefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.DefendingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.DefendingTerritory.Armies.Should().Be(1);
            }

            private void SetActualBattleOutcome(int numberOfAttackDices, int numberOfDefenceDices, BattleOutcome battleOutcome)
            {
                var attackValues = new List<int>();
                var defenceValues = new List<int>();

                _dicesRoller.Roll(numberOfAttackDices, numberOfDefenceDices).Returns(new Dices(attackValues, defenceValues));
                _battleOutcomeCalculator.Battle(attackValues, defenceValues).Returns(battleOutcome);
            }
        }
    }
}