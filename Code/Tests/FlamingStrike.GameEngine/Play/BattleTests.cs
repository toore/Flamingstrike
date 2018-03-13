using System.Collections.Generic;
using System.Reflection;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using NSubstitute;
using Tests.FlamingStrike.GameEngine.Builders;
using Xunit;
using Xunit.Sdk;

namespace Tests.FlamingStrike.GameEngine.Play
{
    public abstract class BattleTests
    {
        private readonly Battle _sut;
        private readonly IDice _dice;
        private readonly IArmiesLostCalculator _armiesLostCalculator;

        protected BattleTests()
        {
            _dice = Substitute.For<IDice>();
            _armiesLostCalculator = Substitute.For<IArmiesLostCalculator>();

            _sut = new Battle(_dice, _armiesLostCalculator);
        }

        public class Uses_correct_number_of_dices_with_respect_to_number_of_armies : BattleTests
        {
            public Uses_correct_number_of_dices_with_respect_to_number_of_armies()
            {
                _dice.Roll(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(new DiceResultBuilder().Build());
                _armiesLostCalculator.Calculate(null, null).ReturnsForAnyArgs(new ArmiesLostBuilder().Build());
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
                var attackingTerritory = new TerritoryBuilder().Armies(armiesInAttackingTerritory).Build();
                var defendingTerritory = new TerritoryBuilder().Build();

                _sut.Attack(attackingTerritory, defendingTerritory);

                _dice.Received().Roll(expectedNumberOfAttackingDices, Arg.Any<int>());
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
                var attackingTerritory = new TerritoryBuilder().Build();
                var defendingTerritory = new TerritoryBuilder().Armies(armiesInDefendingTerritory).Build();

                _sut.Attack(attackingTerritory, defendingTerritory);

                _dice.Received().Roll(Arg.Any<int>(), expectedNumberOfDefendingDices);
            }
        }

        public class Territories_are_updated_after_battle : BattleTests
        {
            [Fact]
            public void Attacker_loses_one()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(3).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(1).Build();
                var attackerLosesOne = new ArmiesLost(1, 0);
                SetArmiesLost(2, 1, attackerLosesOne);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.UpdatedAttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.UpdatedAttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.UpdatedAttackingTerritory.Armies.Should().Be(2);
                battleResult.UpdatedDefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.UpdatedDefendingTerritory.Player.Should().Be(defendingTerritory.Player);
                battleResult.UpdatedDefendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Defender_loses_one()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(2).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(2).Build();
                var defenderLosesOne = new ArmiesLost(0, 1);
                SetArmiesLost(1, 2, defenderLosesOne);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.UpdatedAttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.UpdatedAttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.UpdatedAttackingTerritory.Armies.Should().Be(2);
                battleResult.UpdatedDefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.UpdatedDefendingTerritory.Player.Should().Be(defendingTerritory.Player);
                battleResult.UpdatedDefendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Its_a_draw()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(4).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(2).Build();
                var draw = new ArmiesLost(1, 1);
                SetArmiesLost(3, 2, draw);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.UpdatedAttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.UpdatedAttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.UpdatedAttackingTerritory.Armies.Should().Be(3);
                battleResult.UpdatedDefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.UpdatedDefendingTerritory.Player.Should().Be(defendingTerritory.Player);
                battleResult.UpdatedDefendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Defender_is_defeated_and_occupied()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(2).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(1).Build();
                var defenderLosesOne = new ArmiesLost(0, 1);
                SetArmiesLost(1, 1, defenderLosesOne);

                var battleResult = _sut.Attack(attackingTerritory, defendingTerritory);

                battleResult.UpdatedAttackingTerritory.Region.Should().Be(attackingTerritory.Region);
                battleResult.UpdatedAttackingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.UpdatedAttackingTerritory.Armies.Should().Be(1);
                battleResult.UpdatedDefendingTerritory.Region.Should().Be(defendingTerritory.Region);
                battleResult.UpdatedDefendingTerritory.Player.Should().Be(attackingTerritory.Player);
                battleResult.UpdatedDefendingTerritory.Armies.Should().Be(1);
            }

            private void SetArmiesLost(int numberOfAttackDices, int numberOfDefenceDices, ArmiesLost armiesLost)
            {
                var attackValues = new List<int>();
                var defenceValues = new List<int>();

                _dice.Roll(numberOfAttackDices, numberOfDefenceDices).Returns(new DiceResult(attackValues, defenceValues));
                _armiesLostCalculator.Calculate(attackValues, defenceValues).Returns(armiesLost);
            }
        }
    }
}