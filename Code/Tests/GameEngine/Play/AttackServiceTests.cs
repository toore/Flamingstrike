using System;
using System.Collections.Generic;
using System.Reflection;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Xunit.Sdk;

namespace Tests.GameEngine.Play
{
    public abstract class AttackServiceTests
    {
        private readonly AttackService _sut;
        private readonly IWorldMap _worldMap;
        private readonly IDice _dice;
        private readonly IArmiesLostCalculator _armiesLostCalculator;

        protected AttackServiceTests()
        {
            _worldMap = Substitute.For<IWorldMap>();
            _dice = Substitute.For<IDice>();
            _armiesLostCalculator = Substitute.For<IArmiesLostCalculator>();

            _sut = new AttackService(_worldMap, _dice, _armiesLostCalculator);
        }

        public class Uses_correct_number_of_dices_with_respect_to_number_of_armies : AttackServiceTests
        {
            private readonly PlayerName _player;
            private readonly PlayerName _anotherPlayer;
            private readonly Region _region;
            private readonly Region _anotherRegion;

            public Uses_correct_number_of_dices_with_respect_to_number_of_armies()
            {
                _player = new PlayerName("player");
                _anotherPlayer = new PlayerName("another player");
                _region = Region.Brazil;
                _anotherRegion = Region.NorthAfrica;

                _worldMap.HasBorder(_region, _anotherRegion).Returns(true);

                _dice.Roll(Arg.Any<int>(), Arg.Any<int>()).ReturnsForAnyArgs(new DiceResultBuilder().Build());
                _armiesLostCalculator.CalculateAttackerLosses(null, null).ReturnsForAnyArgs(0);
                _armiesLostCalculator.CalculateDefenderLosses(null, null).ReturnsForAnyArgs(0);
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
                var attackingTerritory = new TerritoryBuilder().Armies(armiesInAttackingTerritory).Region(_region).Player(_player).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(1).Region(_anotherRegion).Player(_anotherPlayer).Build();

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
                var attackingTerritory = new TerritoryBuilder().Armies(2).Region(_region).Player(_player).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(armiesInDefendingTerritory).Region(_anotherRegion).Player(_anotherPlayer).Build();

                _sut.Attack(attackingTerritory, defendingTerritory);

                _dice.Received().Roll(Arg.Any<int>(), expectedNumberOfDefendingDices);
            }
        }

        public class Territories_are_updated_after_battle : AttackServiceTests
        {
            private readonly PlayerName _player;
            private readonly PlayerName _anotherPlayer;
            private readonly Region _region;
            private readonly Region _anotherRegion;

            public Territories_are_updated_after_battle()
            {
                _player = new PlayerName("player");
                _anotherPlayer = new PlayerName("another player");
                _region = Region.Brazil;
                _anotherRegion = Region.NorthAfrica;

                _worldMap.HasBorder(_region, _anotherRegion).Returns(true);
            }

            [Fact]
            public void Attacker_loses_one()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(3).Region(_region).Player(_player).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(1).Region(_anotherRegion).Player(_anotherPlayer).Build();
                MatchDiceRollWithArmiesLost(
                    x => x.NumberOfAttackingDice(2)
                        .NumberOfDefendingDice(1)
                        .AttackerLosses(1));

                var defendingArmyStatus = _sut.Attack(attackingTerritory, defendingTerritory);

                defendingArmyStatus.Should().Be(DefendingArmyStatus.IsAlive);
                attackingTerritory.Region.Should().Be(attackingTerritory.Region);
                attackingTerritory.Name.Should().Be(attackingTerritory.Name);
                attackingTerritory.Armies.Should().Be(2);
                defendingTerritory.Region.Should().Be(defendingTerritory.Region);
                defendingTerritory.Name.Should().Be(defendingTerritory.Name);
                defendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Defender_loses_one()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(2).Region(_region).Player(_player).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(2).Region(_anotherRegion).Player(_anotherPlayer).Build();
                MatchDiceRollWithArmiesLost(
                    x => x.NumberOfAttackingDice(1)
                        .NumberOfDefendingDice(2)
                        .DefenderLosses(1));

                var defendingArmyStatus = _sut.Attack(attackingTerritory, defendingTerritory);

                defendingArmyStatus.Should().Be(DefendingArmyStatus.IsAlive);
                attackingTerritory.Region.Should().Be(attackingTerritory.Region);
                attackingTerritory.Name.Should().Be(attackingTerritory.Name);
                attackingTerritory.Armies.Should().Be(2);
                defendingTerritory.Region.Should().Be(defendingTerritory.Region);
                defendingTerritory.Name.Should().Be(defendingTerritory.Name);
                defendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Its_a_draw()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(4).Region(_region).Player(_player).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(2).Region(_anotherRegion).Player(_anotherPlayer).Build();
                MatchDiceRollWithArmiesLost(
                    x => x.NumberOfAttackingDice(3)
                        .NumberOfDefendingDice(2)
                        .AttackerLosses(1)
                        .DefenderLosses(1));
                var defendingArmyStatus = _sut.Attack(attackingTerritory, defendingTerritory);

                defendingArmyStatus.Should().Be(DefendingArmyStatus.IsAlive);
                attackingTerritory.Region.Should().Be(attackingTerritory.Region);
                attackingTerritory.Name.Should().Be(attackingTerritory.Name);
                attackingTerritory.Armies.Should().Be(3);
                defendingTerritory.Region.Should().Be(defendingTerritory.Region);
                defendingTerritory.Name.Should().Be(defendingTerritory.Name);
                defendingTerritory.Armies.Should().Be(1);
            }

            [Fact]
            public void Defender_is_defeated_and_occupied()
            {
                var attackingTerritory = new TerritoryBuilder().Armies(2).Region(_region).Player(_player).Build();
                var defendingTerritory = new TerritoryBuilder().Armies(1).Region(_anotherRegion).Player(_anotherPlayer).Build();
                MatchDiceRollWithArmiesLost(
                    x => x.NumberOfAttackingDice(1)
                        .NumberOfDefendingDice(1)
                        .DefenderLosses(1));

                var defendingArmyStatus = _sut.Attack(attackingTerritory, defendingTerritory);

                defendingArmyStatus.Should().Be(DefendingArmyStatus.IsEliminated);
                attackingTerritory.Region.Should().Be(attackingTerritory.Region);
                attackingTerritory.Name.Should().Be(attackingTerritory.Name);
                attackingTerritory.Armies.Should().Be(1);
                defendingTerritory.Region.Should().Be(defendingTerritory.Region);
                defendingTerritory.Name.Should().Be(attackingTerritory.Name);
                defendingTerritory.Armies.Should().Be(1);
            }

            private void MatchDiceRollWithArmiesLost(Action<IDiceRollAndArmiesLostStubsHelper> stubActions)
            {
                var diceRollAndLossesBuilder = new DiceRollAndArmiesLostStubsHelper(_dice, _armiesLostCalculator);
                stubActions(diceRollAndLossesBuilder);
                diceRollAndLossesBuilder.ApplyStubs();
            }
        }

        private interface IDiceRollAndArmiesLostStubsHelper
        {
            IDiceRollAndArmiesLostStubsHelper NumberOfAttackingDice(int numberOfAttackingDice);
            IDiceRollAndArmiesLostStubsHelper NumberOfDefendingDice(int numberOfDefendingDice);
            IDiceRollAndArmiesLostStubsHelper AttackerLosses(int attackerLosses);
            IDiceRollAndArmiesLostStubsHelper DefenderLosses(int defenderLosses);
        }

        private class DiceRollAndArmiesLostStubsHelper : IDiceRollAndArmiesLostStubsHelper
        {
            private readonly IDice _dice;
            private readonly IArmiesLostCalculator _armiesLostCalculator;
            private int _numberOfAttackingDice;
            private int _numberOfDefendingDice;
            private int _attackerLosses;
            private int _defenderLosses;

            public DiceRollAndArmiesLostStubsHelper(IDice dice, IArmiesLostCalculator armiesLostCalculator)
            {
                _dice = dice;
                _armiesLostCalculator = armiesLostCalculator;
            }

            public void ApplyStubs()
            {
                var attackValues = new List<int>();
                var defenceValues = new List<int>();

                _dice.Roll(_numberOfAttackingDice, _numberOfDefendingDice).Returns(new DiceResult(attackValues, defenceValues));
                _armiesLostCalculator.CalculateAttackerLosses(attackValues, defenceValues).Returns(_attackerLosses);
                _armiesLostCalculator.CalculateDefenderLosses(attackValues, defenceValues).Returns(_defenderLosses);
            }

            public IDiceRollAndArmiesLostStubsHelper NumberOfAttackingDice(int numberOfAttackingDice)
            {
                _numberOfAttackingDice = numberOfAttackingDice;
                return this;
            }

            public IDiceRollAndArmiesLostStubsHelper NumberOfDefendingDice(int numberOfDefendingDice)
            {
                _numberOfDefendingDice = numberOfDefendingDice;
                return this;
            }

            public IDiceRollAndArmiesLostStubsHelper AttackerLosses(int attackerLosses)
            {
                _attackerLosses = attackerLosses;
                return this;
            }

            public IDiceRollAndArmiesLostStubsHelper DefenderLosses(int defenderLosses)
            {
                _defenderLosses = defenderLosses;
                return this;
            }
        }
    }
}